using RestSharp;
using RestSharp.Authenticators;
using SwaggerFile.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SwaggerFile.Processor
{
    public class ApimProcessor
    {
        //protected string? ApimUrl;
        //protected string? GrantType;
        //protected string? ClientId;
        //protected string? ClientUuid;
        //protected string? Password;
        //protected string? Username;
        //protected string? BackendUrl;

        //private readonly Dictionary<string, string> _devApplicationsIds = new()
        //{
        //    ["Rmq Ariane"] = "24fe03ae-bbe5-447d-8177-5362e233a0a7",
        //    ["Rmq OGC"] = "59b8510a-1441-4d9a-b5a9-ba33cd6be075",
        //    ["SageX3"] = "bee75d25-45c9-4ef2-870b-f2c739aadcbd",
        //    ["Homebox.fr"] = "5ef13e58-c86f-45f2-8208-921420d15769"
        //};

        //private readonly Dictionary<string, string> _recApplicationsIds = new()
        //{
        //    ["Rmq Ariane"] = "4cb4d9b9-2460-4415-970b-0328b0a4746d",
        //    ["Rmq OGC"] = "a36dcc82-2188-4e41-b04d-95ddb77e0a98",
        //    ["SageX3"] = "1a46228a-7e29-4a8b-84b9-5f3d873fd2a0",
        //    ["Homebox.fr"] = "3e6419b7-2b5b-4f16-bfee-406122981730"
        //};

        //private readonly Dictionary<string, string> _pprApplicationsIds = new()
        //{
        //    ["Rmq Ariane"] = "16089654-c2b9-49d7-b64d-56a63dcb9d9d",
        //    ["Rmq OGC"] = "1229fb8a-6d45-41a4-b085-eac59e08554f",
        //    ["SageX3"] = "2ff80063-37c1-41c1-95c7-bbb9c386344f",
        //    ["Homebox.fr"] = "65547711-cd00-40a1-9af5-a63b23161251"
        //};

        //private readonly Dictionary<string, string> _prodApplicationsIds = new()
        //{
        //    ["Rmq Ariane"] = "f7d1f9bd-c803-4128-904a-38fe97b1704e",
        //    ["Rmq OGC"] = "2f42a49d-9d47-4878-b429-2c3109228ecf",
        //    ["SageX3"] = "c60cf84c-87de-40ba-8489-f8c1653243e9",
        //    ["Homebox.fr"] = "1e49ea9e-8cf7-45f5-9c02-acc06c77d587"
        //};


        //private Dictionary<string, string> _currentApplicationsIds;
        //private string? _token;

        //public ApimProcessor(Environnement env)
        //{
        //    Init(env);
        //}

        //private void Init(Environnement env)
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("launchSettings.json", optional: true)
        //        .AddJsonFile("appsettings.json", optional: true);

        //    var environmentSettingsFile = env switch
        //    {
        //        Environnement.Dev => "appsettings.Development.json",
        //        Environnement.Rec => "appsettings.Recette.json",
        //        Environnement.Ppr => "appsettings.Preprod.json",
        //        Environnement.Prod => null
        //    };

        //    if (environmentSettingsFile is not null)
        //    {
        //        configuration.AddJsonFile(environmentSettingsFile, optional: false);
        //    }

        //    configuration.AddEnvironmentVariables();

        //    var config = configuration.Build();

        //    _currentApplicationsIds = env switch
        //    {
        //        Environnement.Dev => _devApplicationsIds,
        //        Environnement.Rec => _recApplicationsIds,
        //        Environnement.Ppr => _pprApplicationsIds,
        //        Environnement.Prod => _prodApplicationsIds,
        //        _ => throw new ArgumentOutOfRangeException(nameof(env), env, null)
        //    };

        //    ApimUrl = config.GetSection("Apim")["Url"]!;
        //    GrantType = config.GetSection("Apim")["GrantType"]!;
        //    ClientId = config.GetSection("Apim")["ClientId"]!;
        //    ClientUuid = config.GetSection("Apim")["ClientUUID"]!;
        //    Password = config.GetSection("Apim")["Password"]!;
        //    Username = config.GetSection("Apim")["Username"]!;
        //    BackendUrl = config.GetSection("Apim")["BaseBackendUrl"]!;
        //}

        //public async Task<string?> GetToken()
        //{
        //    using var client = new HttpClient();
        //    var requestContent = new FormUrlEncodedContent([
        //        new KeyValuePair<string, string?>("grant_type", GrantType),
        //        new KeyValuePair<string, string?>("client_id", ClientId),
        //        new KeyValuePair<string, string?>("password", Password),
        //        new KeyValuePair<string, string?>("username", Username)
        //    ]);

        //    var url = $"{ApimUrl}/keycloak/realms/ariane/protocol/openid-connect/token";
        //    var response = await client.PostAsync(url, requestContent);

        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(responseContent);
        //        _token = tokenInfo!.access_token;
        //        return tokenInfo!.access_token;
        //    }

        //    var message = await response.Content.ReadAsStringAsync();
        //    throw new Exception("CannotObtainToken");
        //}

        //public async Task SetupRmq(IDictionary<string, IList<QueueBindingConfig>> dictionary)
        //{
        //    var options = new RestClientOptions(ApimUrl)
        //    {
        //        Authenticator = new JwtAuthenticator(_token)
        //    };

        //    using var restClient = new RestClient(options);


        //    foreach (var b in dictionary.SelectMany(x => x.Value))
        //    {
        //        if (b.DevWebHook == null) continue;

        //        restClient.DefaultParameters.RemoveParameter("KeyId", ParameterType.HttpHeader);
        //        restClient.AddDefaultHeader("KeyId", _currentApplicationsIds[b.From]);

        //        var request = new RestRequest("/rmq/setup", Method.Post);
        //        request.AddJsonBody(new
        //        {
        //            destination = b.DestinationJms,
        //            webhook = b.DevWebHook,
        //            maxretry = 2,
        //            description = b.ToString()
        //        });
        //        var response = await restClient.ExecuteAsync(request);
        //        Console.WriteLine(response.IsSuccessStatusCode
        //            ? $"{b.DestinationJms} a été créé."
        //            : $"{b.DestinationJms} error !!!");
        //    }
        //}


        /// <summary>
        /// Génère le fichier .bindings à partir du contenu CSV et l'écrit sur disque.
        /// </summary>
        public async static Task<byte[]> GenerateBindingsFileAsync(List<QueueBindingConfig> rows)
        {
            string content = GenerateBindingsFileContent(rows);
            //await File.WriteAllTextAsync(outputPath, content);
            Console.WriteLine("Fichier .bindings généré");
            return Encoding.UTF8.GetBytes(content);
        }

        // ---------- Génération du fichier .bindings (JMS / RMQObjectFactory) ----------

        /// <summary>
        /// Génère le contenu au format .bindings pour chaque queue applicative du CSV,
        /// ainsi que pour sa DLQ associée si Dead letter Exchange / Dead letter Queue sont renseignés.
        /// Le DestinationJMS de la DLQ = DestinationJMS d'origine + suffixe ".dlq".
        /// </summary>
        public static string GenerateBindingsFileContent(List<QueueBindingConfig> rows)
        {

            var sb = new StringBuilder();

            sb.Append(@"# ---------------------------------------------------------------------- 
# Creation du fichier .bindings v1.7
# Chemin d'accès : /opt/axway/apigateway/jms2 
# ---------------------------------------------------------------------- 
ConnectionFactory/ClassName=javax.jms.ConnectionFactory
ConnectionFactory/FactoryName=com.rabbitmq.jms.admin.RMQObjectFactory
ConnectionFactory/RefAddr/0/Content=jms/ConnectionFactory
ConnectionFactory/RefAddr/0/Type=name
ConnectionFactory/RefAddr/0/Encoding=String
# ---------------------------------------------
ConnectionFactory/RefAddr/1/Content=javax.jms.ConnectionFactory
ConnectionFactory/RefAddr/1/Type=type
ConnectionFactory/RefAddr/1/Encoding=String
# ---------------------------------------------
ConnectionFactory/RefAddr/2/Content=com.rabbitmq.jms.admin.RMQObjectFactory
ConnectionFactory/RefAddr/2/Type=factory
ConnectionFactory/RefAddr/2/Encoding=String
# ---------------------------------------------
ConnectionFactory/RefAddr/3/Content=172.19.241.2
ConnectionFactory/RefAddr/3/Type=host
ConnectionFactory/RefAddr/3/Encoding=String
# ---------------------------------------------
ConnectionFactory/RefAddr/4/Content=31672
ConnectionFactory/RefAddr/4/Type=port
ConnectionFactory/RefAddr/4/Encoding=String
# ****************************************************************************************************************************************************************");

            foreach (var row in rows)
            {
                // Bloc principal : exchange applicatif -> queue applicative
                sb.Append(BuildBindingBlock(
                    destinationJms: row.DestinationJms,
                    exchangeName: row.Exchange,
                    routingKey: row.RoutingKey,
                    queueName: row.QueueName
                ));
                sb.AppendLine();

                bool hasDlx = !string.IsNullOrWhiteSpace(row.DeadLetterExchange)
                           && !string.IsNullOrWhiteSpace(row.DeadLetterQueue);

                if (hasDlx)
                {
                    string dlqDestinationJms = $"{row.DestinationJms}DLQ";

                    // Bloc DLQ : DLX -> DLQ, même routing key que le binding principal
                    sb.Append(BuildBindingBlock(
                        destinationJms: dlqDestinationJms,
                        exchangeName: row.DeadLetterExchange,
                        routingKey: row.RoutingKey,
                        queueName: row.DeadLetterQueue
                    ));
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private static string BuildBindingBlock(string destinationJms, string exchangeName, string routingKey, string queueName)
        {
            return
$@"# ****************************************************************************************************************************************************************
{destinationJms}/ClassName=com.rabbitmq.jms.admin.RMQDestination
{destinationJms}/FactoryName=com.rabbitmq.jms.admin.RMQObjectFactory
# ---------------------------------------------
{destinationJms}/RefAddr/0/Content=jms/Queue
{destinationJms}/RefAddr/0/Type=name
{destinationJms}/RefAddr/0/Encoding=String
# ---------------------------------------------
{destinationJms}/RefAddr/1/Content=javax.jms.Queue
{destinationJms}/RefAddr/1/Type=type
{destinationJms}/RefAddr/1/Encoding=String
# ---------------------------------------------
{destinationJms}/RefAddr/2/Content=com.rabbitmq.jms.admin.RMQObjectFactory
{destinationJms}/RefAddr/2/Type=factory
{destinationJms}/RefAddr/2/Encoding=String
# ---------------------------------------------
{destinationJms}/RefAddr/3/Content={destinationJms}
{destinationJms}/RefAddr/3/Type=destinationName
{destinationJms}/RefAddr/3/Encoding=String
# ---------------------------------------------
{destinationJms}/RefAddr/4/Content=true
{destinationJms}/RefAddr/4/Type=amqp
{destinationJms}/RefAddr/4/Encoding=String
# ---------------------------------------------	
{destinationJms}/RefAddr/5/Content={exchangeName}
{destinationJms}/RefAddr/5/Type=amqpExchangeName
{destinationJms}/RefAddr/5/Encoding=String
# ---------------------------------------------	
{destinationJms}/RefAddr/6/Content={routingKey}
{destinationJms}/RefAddr/6/Type=amqpRoutingKey
{destinationJms}/RefAddr/6/Encoding=String
# ---------------------------------------------	
{destinationJms}/RefAddr/7/Content={queueName}
{destinationJms}/RefAddr/7/Type=amqpQueueName
{destinationJms}/RefAddr/7/Encoding=String
# ****************************************************************************************************************************************************************
";
        }
    }

    //public class TokenInfo
    //{
    //    public string access_token { get; set; }
    //}
}
