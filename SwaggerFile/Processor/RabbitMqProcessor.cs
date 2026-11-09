using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using RestSharp;

namespace SwaggerFile.Processor
{
    // ===================== MODELES =====================

    public class QueueBindingConfig
    {
        //[CsvHelper.Configuration.Attributes.Name("application")]
        //public string From { get; set; }

        [CsvHelper.Configuration.Attributes.Name("application")]
        public string To { get; set; }

        //public string What { get; set; }

        //[CsvHelper.Configuration.Attributes.Name("évènement")]
        //public string Event { get; set; }

        [CsvHelper.Configuration.Attributes.Name("exchange")]
        public string Exchange { get; set; }

        [CsvHelper.Configuration.Attributes.Name("routing_key")]
        public string RoutingKey { get; set; }

        [CsvHelper.Configuration.Attributes.Name("queue")]
        public string QueueName { get; set; }

        [CsvHelper.Configuration.Attributes.Name("destination")]
        public string DestinationJms { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Dead letter Exchange")]
        public string DeadLetterExchange { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Dead letter Queue")]
        public string DeadLetterQueue { get; set; } 

        //[CsvHelper.Configuration.Attributes.Name("DEV")]
        //public string DebWebhook { get; set; }

        //public override string ToString()
        //{
        //    return $"From {From} to {To} : {Event} {What}";
        //}
    }

    //public class ExchangeGrouping
    //{
    //    public string ExchangeName { get; set; }
    //    public IEnumerable<QueueBindingConfig> QueueBindingConfigs { get; set; }

    //    public ExchangeGrouping(string name, IEnumerable<QueueBindingConfig> bindings)
    //    {
    //        ExchangeName = name;
    //        QueueBindingConfigs = bindings;
    //    }
    //}

    // ===================== SERVICE PRINCIPAL =====================

    public class RabbitMqProcessor
    {
        // ---------- Entrée : lecture du CSV et dispatch par exchange ----------

        public async Task ConfigureRmqFromCsvAsync(RestClient client, string vhost, string? csvPath, string? csvContent, List<QueueBindingConfig> rows)
        {
            foreach (var queueBinding in BuildQueueBindings(rows))
            {
                try
                {
                    await SetupExchangeWithMultipleQueuesAsync(
                        client,
                        vhost,
                        queueBinding.Key,
                        exchangeType: "topic",
                        queueBinding.Value.ToList()
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Échec pour l'exchange '{queueBinding.Key}' : {ex.Message}");
                    // On continue avec les autres exchanges même si celui-ci échoue
                }
            }
        }

        public static IDictionary<string, IList<QueueBindingConfig>> BuildQueueBindings(List<QueueBindingConfig> rows)
        {
            Console.WriteLine($"{rows.Count} lignes lues depuis le CSV.");

            var groupedByExchange = rows.GroupBy(r => r.Exchange);
            var result = new Dictionary<string, IList<QueueBindingConfig>>();

            foreach (var group in groupedByExchange)
            {
                string exchangeName = group.Key;

                var queueBindings = group.Select(r => new QueueBindingConfig
                {
                    QueueName = r.QueueName,
                    RoutingKey = r.RoutingKey,
                    DeadLetterExchange = r.DeadLetterExchange,
                    DeadLetterQueue = r.DeadLetterQueue,
                    //Event = r.Event,
                    DestinationJms = r.DestinationJms,
                    //From = r.From
                }).ToList();

                result.Add(exchangeName, queueBindings);
                Console.WriteLine($"--- Exchange '{exchangeName}' : {queueBindings.Count} queue(s) ---");
            }
            return result;
        }

       
        // ---------- Setup complet : exchange + queues + DLX/DLQ + bindings, avec rollback ----------

        public async Task SetupExchangeWithMultipleQueuesAsync(
            RestClient client,
            string vhost,
            string exchangeName,
            string exchangeType,
            List<QueueBindingConfig> queueBindings)
        {
            string vhostEncoded = Uri.EscapeDataString(vhost);

            bool exchangeCreated = false;
            var createdDlxExchanges = new HashSet<string>();
            var createdQueues = new List<string>();
            var createdBindings = new List<(string queue, string propertiesKey)>();

            try
            {
                // ===== 1. Exchange principal =====
                await EnsureRecreatedExchangeAsync(client, vhostEncoded, vhost, exchangeName, exchangeType);
                exchangeCreated = true;
                Console.WriteLine($"Exchange '{exchangeName}' créé.");

                // ===== 2. Pour chaque queue =====
                foreach (var config in queueBindings)
                {
                    bool hasDlx = !string.IsNullOrWhiteSpace(config.DeadLetterExchange)
                               && !string.IsNullOrWhiteSpace(config.DeadLetterQueue);

                    // --- 2a. DLX + DLQ si renseignés ---
                    if (hasDlx)
                    {
                        if (!createdDlxExchanges.Contains(config.DeadLetterExchange))
                        {
                            await EnsureRecreatedExchangeAsync(client, vhostEncoded, vhost, config.DeadLetterExchange, "topic");
                            createdDlxExchanges.Add(config.DeadLetterExchange);
                            Console.WriteLine($"DLX '{config.DeadLetterExchange}' créé.");
                        }

                        // Création de la DLQ (sans arguments particuliers)
                        await EnsureRecreatedQueueAsync(client, vhostEncoded, vhost, config.DeadLetterQueue, arguments: new { });
                        createdQueues.Add(config.DeadLetterQueue);
                        Console.WriteLine($"DLQ '{config.DeadLetterQueue}' créée.");

                        // Binding DLX -> DLQ avec la MEME routing key que l'exchange principal -> queue
                        // (respecte la même logique de routing que le CSV, évite les collisions
                        // tant que chaque routing key du CSV est unique)
                        string dlqPropertiesKey = await CreateBindingAsync(
                            client, vhostEncoded, config.DeadLetterExchange, config.DeadLetterQueue, config.RoutingKey
                        );
                        createdBindings.Add((config.DeadLetterQueue, dlqPropertiesKey));
                        Console.WriteLine($"Binding DLX créé : '{config.DeadLetterExchange}' --[{config.RoutingKey}]--> '{config.DeadLetterQueue}'");
                    }

                    // --- 2b. Queue d'origine, avec arguments DLX si applicable ---
                    object queueArguments = hasDlx
                        ? new
                        {
                            x_dead_letter_exchange = config.DeadLetterExchange,
                            x_dead_letter_routing_key = config.RoutingKey
                        }
                        : new { };

                    await EnsureRecreatedQueueAsync(client, vhostEncoded, vhost, config.QueueName, queueArguments);
                    createdQueues.Add(config.QueueName);
                    Console.WriteLine($"Queue '{config.QueueName}' créée" + (hasDlx ? $" (DLX: {config.DeadLetterExchange})" : "") + ".");

                    // --- 2c. Binding exchange principal -> queue ---
                    string propertiesKey = await CreateBindingAsync(
                        client, vhostEncoded, exchangeName, config.QueueName, config.RoutingKey
                    );
                    createdBindings.Add((config.QueueName, propertiesKey));
                    Console.WriteLine($"Binding créé : '{exchangeName}' --[{config.RoutingKey}]--> '{config.QueueName}'");
                }

                Console.WriteLine("Configuration complète terminée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur détectée : {ex.Message}");
                Console.WriteLine("Démarrage du rollback...");

                await RollbackAsync(client, vhostEncoded, exchangeName, exchangeCreated, createdDlxExchanges, createdQueues, createdBindings);

                throw;
            }
        }

        // ===================== HELPERS =====================

        private async Task EnsureRecreatedExchangeAsync(RestClient client, string vhostEncoded, string vhost, string exchangeName, string type)
        {
            var checkRequest = new RestRequest($"api/exchanges/{vhostEncoded}/{exchangeName}");
            var checkResponse = await client.ExecuteAsync(checkRequest);

            if (checkResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var deleteRequest = new RestRequest($"api/exchanges/{vhostEncoded}/{exchangeName}", Method.Delete);
                var deleteResponse = await client.ExecuteAsync(deleteRequest);
                if (!deleteResponse.IsSuccessful)
                    throw new Exception($"Échec suppression exchange '{exchangeName}' : {deleteResponse.StatusCode} - {deleteResponse.Content}");
            }

            var createRequest = new RestRequest($"api/exchanges/{vhostEncoded}/{exchangeName}", Method.Put);
            createRequest.AddJsonBody(new
            {
                vhost,
                name = exchangeName,
                type,
                durable = true,
                auto_delete = false,
                @internal = false,
                arguments = new { }
            });

            var createResponse = await client.ExecuteAsync(createRequest);
            if (!createResponse.IsSuccessful)
                throw new Exception($"Échec création exchange '{exchangeName}' : {createResponse.StatusCode} - {createResponse.Content}");
        }

        private async Task EnsureRecreatedQueueAsync(RestClient client, string vhostEncoded, string vhost, string queueName, object arguments)
        {
            var checkRequest = new RestRequest($"api/queues/{vhostEncoded}/{queueName}");
            var checkResponse = await client.ExecuteAsync(checkRequest);

            if (checkResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var deleteRequest = new RestRequest($"api/queues/{vhostEncoded}/{queueName}", Method.Delete);
                var deleteResponse = await client.ExecuteAsync(deleteRequest);
                if (!deleteResponse.IsSuccessful)
                    throw new Exception($"Échec suppression queue '{queueName}' : {deleteResponse.StatusCode} - {deleteResponse.Content}");
            }

            var createRequest = new RestRequest($"api/queues/{vhostEncoded}/{queueName}", Method.Put);
            createRequest.AddJsonBody(new
            {
                vhost,
                name = queueName,
                durable = true,
                auto_delete = false,
                arguments
            });

            var createResponse = await client.ExecuteAsync(createRequest);
            if (!createResponse.IsSuccessful)
                throw new Exception($"Échec création queue '{queueName}' : {createResponse.StatusCode} - {createResponse.Content}");
        }

        private async Task<string> CreateBindingAsync(RestClient client, string vhostEncoded, string exchangeName, string queueName, string routingKey)
        {
            var bindingRequest = new RestRequest($"api/bindings/{vhostEncoded}/e/{exchangeName}/q/{queueName}", Method.Post);
            bindingRequest.AddJsonBody(new { routing_key = routingKey, arguments = new { } });

            var bindingResponse = await client.ExecuteAsync(bindingRequest);
            if (!bindingResponse.IsSuccessful)
                throw new Exception($"Échec création binding '{exchangeName}' -> '{queueName}' : {bindingResponse.StatusCode} - {bindingResponse.Content}");

            var locationHeader = bindingResponse.Headers?
                .FirstOrDefault(h => h.Name.Equals("Location", StringComparison.OrdinalIgnoreCase));

            if (locationHeader?.Value != null)
            {
                string location = locationHeader.Value.ToString();
                return Uri.UnescapeDataString(location.Substring(location.LastIndexOf('/') + 1));
            }

            return routingKey; // fallback si le header Location est absent
        }

        private static async Task RollbackAsync(
            RestClient client,
            string vhostEncoded,
            string exchangeName,
            bool exchangeCreated,
            HashSet<string> createdDlxExchanges,
            List<string> createdQueues,
            List<(string queue, string propertiesKey)> createdBindings)
        {
            // 1. Bindings
            foreach (var (queue, propertiesKey) in createdBindings)
            {
                try
                {
                    var req = new RestRequest($"api/bindings/{vhostEncoded}/e/{exchangeName}/q/{queue}/{Uri.EscapeDataString(propertiesKey)}", Method.Delete);
                    await client.ExecuteAsync(req);
                    Console.WriteLine($"[Rollback] Binding '{queue}' supprimé.");
                }
                catch (Exception ex) { Console.WriteLine($"[Rollback] Échec binding '{queue}' : {ex.Message}"); }
            }

            // 2. Queues (y compris les DLQ, stockées dans la même liste)
            foreach (var queue in createdQueues)
            {
                try
                {
                    var req = new RestRequest($"api/queues/{vhostEncoded}/{queue}", Method.Delete);
                    await client.ExecuteAsync(req);
                    Console.WriteLine($"[Rollback] Queue '{queue}' supprimée.");
                }
                catch (Exception ex) { Console.WriteLine($"[Rollback] Échec queue '{queue}' : {ex.Message}"); }
            }

            // 3. DLX
            foreach (var dlx in createdDlxExchanges)
            {
                try
                {
                    var req = new RestRequest($"api/exchanges/{vhostEncoded}/{dlx}", Method.Delete);
                    await client.ExecuteAsync(req);
                    Console.WriteLine($"[Rollback] DLX '{dlx}' supprimé.");
                }
                catch (Exception ex) { Console.WriteLine($"[Rollback] Échec DLX '{dlx}' : {ex.Message}"); }
            }

            // 4. Exchange principal
            if (exchangeCreated)
            {
                try
                {
                    var req = new RestRequest($"api/exchanges/{vhostEncoded}/{exchangeName}", Method.Delete);
                    await client.ExecuteAsync(req);
                    Console.WriteLine($"[Rollback] Exchange '{exchangeName}' supprimé.");
                }
                catch (Exception ex) { Console.WriteLine($"[Rollback] Échec exchange '{exchangeName}' : {ex.Message}"); }
            }

            Console.WriteLine("Rollback terminé.");
        }
    }
}