using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using SwaggerFile.Models;

namespace SwaggerFile.Processor;

/// <summary>Type de destination JMS : Dstc = consommation (RabbitMQ -> application), Dstp = production (application -> RabbitMQ).</summary>
public enum DestinationKind
{
    Dstc,
    Dstp
}

/// <summary>Une ligne du CSV axway-apim-cible-*.csv.</summary>
public sealed record RmqDestination(
    string Name,
    DestinationKind Kind,
    string Producer,
    string Consumer,
    string Event,
    string Exchange,
    string RoutingKey,
    string Queue);

/// <summary>
/// Résultat de l'appel HTTP pour une destination et un producteur.
/// Une destination dont la colonne producteur contient plusieurs applications ("A | B") produit un résultat par application.
/// </summary>
public sealed record SendResult(
    RmqDestination Destination,
    string? Producer,
    HttpStatusCode? StatusCode,
    bool Success,
    long ElapsedMs,
    string? Error,
    string? ResponseBody);

/// <summary>
/// Envoie une requête POST /rmq/send par destination JMS (et par producteur) pour vérifier qu'elles sont toutes joignables.
/// Équivalent du curl :
///   curl --location 'https://.../rmq/send' --header 'KeyId: ...' --header 'DestinationRmq: &lt;destination&gt;' --data '{...}'
/// Nécessite le package NuGet RestSharp (version 107 ou supérieure).
/// </summary>
public sealed class RmqDestinationTester : IDisposable
{
    protected string? ApimUrl;
    protected string? GrantType;
    protected string? ClientId;
    protected string? ClientUuid;
    protected string? Password;
    protected string? Username;
    protected string? BackendUrl;
    private Dictionary<string, string> _currentApplicationsIds;
    private string? _token;

    private const string SendResource = "rmq/send";

    private readonly RestClient _client;
    private readonly Func<RmqDestination, string> _payloadProvider;
    private readonly TimeSpan _timeout;
    private readonly Action<string> _log;

    private readonly Dictionary<string, string> _devApplicationsIds = new()
    {
        ["Rmq Ariane"] = "24fe03ae-bbe5-447d-8177-5362e233a0a7",
        ["OGC"] = "59b8510a-1441-4d9a-b5a9-ba33cd6be075",
        ["Sage X3"] = "bee75d25-45c9-4ef2-870b-f2c739aadcbd",
        ["Website.Backend"] = "5ef13e58-c86f-45f2-8208-921420d15769"
    };

    private readonly Dictionary<string, string> _recApplicationsIds = new()
    {
        ["Rmq Ariane"] = "4cb4d9b9-2460-4415-970b-0328b0a4746d",
        ["OGC"] = "a36dcc82-2188-4e41-b04d-95ddb77e0a98",
        ["Sage X3"] = "1a46228a-7e29-4a8b-84b9-5f3d873fd2a0",
        ["Website.Backend"] = "3e6419b7-2b5b-4f16-bfee-406122981730"
    };

    private readonly Dictionary<string, string> _pprApplicationsIds = new()
    {
        ["Rmq Ariane"] = "16089654-c2b9-49d7-b64d-56a63dcb9d9d",
        ["OGC"] = "1229fb8a-6d45-41a4-b085-eac59e08554f",
        ["Sage X3"] = "2ff80063-37c1-41c1-95c7-bbb9c386344f",
        ["Website.Backend"] = "65547711-cd00-40a1-9af5-a63b23161251"
    };

    private readonly Dictionary<string, string> _prodApplicationsIds = new()
    {
        ["Rmq Ariane"] = "f7d1f9bd-c803-4128-904a-38fe97b1704e",
        ["OGC"] = "2f42a49d-9d47-4878-b429-2c3109228ecf",
        ["Sage X3"] = "c60cf84c-87de-40ba-8489-f8c1653243e9",
        ["Website.Backend"] = "1e49ea9e-8cf7-45f5-9c02-acc06c77d587"
    };

    /// <param name="env">Environnement ciblé (détermine le fichier appsettings et les KeyId des applications).</param>
    /// <param name="payloadProvider">
    /// Fournit le corps JSON à envoyer pour une destination donnée.
    /// Par défaut : <see cref="DefaultPayload"/>.
    /// </param>
    /// <param name="timeout">Délai maximal par requête (30 s par défaut).</param>
    /// <param name="log">Sortie des messages de progression (Console.WriteLine par défaut).</param>
    public RmqDestinationTester(
        Environnement env = Environnement.Dev,
        Func<RmqDestination, string>? payloadProvider = null,
        TimeSpan? timeout = null,
        Action<string>? log = null)
    {
        Init(env);

        _payloadProvider = payloadProvider ?? DefaultPayload;
        _timeout = timeout ?? TimeSpan.FromSeconds(30);
        _log = log ?? Console.WriteLine;
        GetToken().GetAwaiter().GetResult();

        var options = new RestClientOptions(ApimUrl.TrimEnd('/'))
        {
            Authenticator = new JwtAuthenticator(_token)
        };
        _client = new RestClient(options);
    }

    private void Init(Environnement env)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("launchSettings.json", optional: true)
            .AddJsonFile("appsettings.json", optional: true);

        var environmentSettingsFile = env switch
        {
            Environnement.Dev => "appsettings.Development.json",
            Environnement.Rec => "appsettings.Recette.json",
            Environnement.Ppr => "appsettings.Preprod.json",
            Environnement.Prod => null
        };

        if (environmentSettingsFile is not null)
        {
            configuration.AddJsonFile(environmentSettingsFile, optional: false);
        }

        configuration.AddEnvironmentVariables();

        var config = configuration.Build();

        _currentApplicationsIds = env switch
        {
            Environnement.Dev => _devApplicationsIds,
            Environnement.Rec => _recApplicationsIds,
            Environnement.Ppr => _pprApplicationsIds,
            Environnement.Prod => _prodApplicationsIds,
            _ => throw new ArgumentOutOfRangeException(nameof(env), env, null)
        };

        ApimUrl = config.GetSection("Apim")["Url"]!;
        GrantType = config.GetSection("Apim")["GrantType"]!;
        ClientId = config.GetSection("Apim")["ClientId"]!;
        ClientUuid = config.GetSection("Apim")["ClientUUID"]!;
        Password = config.GetSection("Apim")["Password"]!;
        Username = config.GetSection("Apim")["Username"]!;
        BackendUrl = config.GetSection("Apim")["BaseBackendUrl"]!;
    }

    public async Task<string?> GetToken()
    {
        using var client = new HttpClient();
        var requestContent = new FormUrlEncodedContent([
            new KeyValuePair<string, string?>("grant_type", GrantType),
            new KeyValuePair<string, string?>("client_id", ClientId),
            new KeyValuePair<string, string?>("password", Password),
            new KeyValuePair<string, string?>("username", Username)
        ]);

        var url = $"{ApimUrl}/keycloak/realms/ariane/protocol/openid-connect/token";
        var response = await client.PostAsync(url, requestContent);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(responseContent);
            _token = tokenInfo!.access_token;
            return tokenInfo!.access_token;
        }

        var message = await response.Content.ReadAsStringAsync();
        throw new Exception("CannotObtainToken");
    }

    /// <summary>
    /// Corps par défaut : le message de l'exemple pour les destinations de facturation, "{}" pour les autres.
    /// À adapter : chaque événement attend en réalité son propre contrat de message.
    /// </summary>
    public static string DefaultPayload(RmqDestination destination)
    {
        if (destination.Name.Contains("InvoicingInvoice", StringComparison.OrdinalIgnoreCase))
        {
            return """
                   {
                       "billingId": "faad8fed-a6d5-43ec-a147-7a5423380728",
                       "clientId": 591
                   }
                   """;
        }

        return "{}";
    }

    // -------------------------------------------------------------------------
    // Lecture du CSV
    // -------------------------------------------------------------------------

    /// <summary>
    /// Charge les destinations depuis le CSV (séparateur ';').
    /// Colonnes lues : destination, type, producteur, consommateur, évènement, exchange, routing_key, queue.
    /// </summary>
    /// <param name="kinds">Types à conserver (par défaut : Dstp et Dstc).</param>
    public static List<RmqDestination> LoadFromCsv(string fileName, params DestinationKind[] kinds)
    {
        var wanted = kinds.Length > 0 ? kinds : [DestinationKind.Dstp, DestinationKind.Dstc];

        var lines = File.ReadAllLines(fileName, Encoding.UTF8)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();
        if (lines.Count < 2) throw new InvalidDataException($"CSV vide ou sans données : {fileName}");

        var header = SplitCsvLine(lines[0].TrimStart('\uFEFF'));
        int Col(params string[] names)
        {
            for (var i = 0; i < header.Count; i++)
            {
                var h = header[i].Trim();
                if (names.Any(n => string.Equals(n, h, StringComparison.OrdinalIgnoreCase))) return i;
            }

            return -1;
        }

        var iName = Col("destination");
        var iType = Col("type");
        if (iName < 0 || iType < 0)
            throw new InvalidDataException("Colonnes 'destination' et 'type' obligatoires dans le CSV.");

        var iProducer = Col("producteur");
        var iConsumer = Col("consommateur");
        var iEvent = Col("évènement", "evenement", "event");
        var iExchange = Col("exchange");
        var iRoutingKey = Col("routing_key");
        var iQueue = Col("queue");

        string Get(List<string> f, int i) => i >= 0 && i < f.Count ? f[i].Trim() : string.Empty;

        var result = new List<RmqDestination>();
        foreach (var line in lines.Skip(1))
        {
            var f = SplitCsvLine(line);
            var name = Get(f, iName);
            if (name.Length == 0) continue;
            if (!Enum.TryParse<DestinationKind>(Get(f, iType), ignoreCase: true, out var kind)) continue;
            if (!wanted.Contains(kind)) continue;

            result.Add(new RmqDestination(
                name, kind,
                Get(f, iProducer), Get(f, iConsumer), Get(f, iEvent),
                Get(f, iExchange), Get(f, iRoutingKey), Get(f, iQueue)));
        }

        return result;
    }

    /// <summary>Découpe une ligne CSV ';' en gérant les champs entre guillemets.</summary>
    private static List<string> SplitCsvLine(string line)
    {
        var fields = new List<string>();
        var sb = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (inQuotes)
            {
                if (c == '"' && i + 1 < line.Length && line[i + 1] == '"') { sb.Append('"'); i++; }
                else if (c == '"') inQuotes = false;
                else sb.Append(c);
            }
            else if (c == '"') inQuotes = true;
            else if (c == ';') { fields.Add(sb.ToString()); sb.Clear(); }
            else sb.Append(c);
        }

        fields.Add(sb.ToString());
        return fields;
    }

    // -------------------------------------------------------------------------
    // Envoi des requêtes
    // -------------------------------------------------------------------------

    /// <summary>Préfixe des producteurs Ariane dans le CSV (Ariane.CustomerService, Ariane.BillingService, ...).</summary>
    private const string ArianeProducerPrefix = "Ariane.";

    /// <summary>Clé du dictionnaire des KeyId correspondant à l'application Ariane.</summary>
    private const string ArianeApplicationKey = "Rmq Ariane";

    /// <summary>
    /// Retrouve le KeyId d'un producteur du CSV :
    /// 1. correspondance exacte avec le dictionnaire (Sage X3, OGC, Website.Backend, ...) ;
    /// 2. sinon, tout producteur "Ariane.XXX" utilise le KeyId d'Ariane ("Rmq Ariane").
    /// </summary>
    private bool TryResolveKeyId(string producer, out string keyId)
    {
        if (_currentApplicationsIds.TryGetValue(producer, out keyId!))
            return true;

        if (producer.StartsWith(ArianeProducerPrefix, StringComparison.OrdinalIgnoreCase) && _currentApplicationsIds.TryGetValue(ArianeApplicationKey, out keyId!))
            return true;

        keyId = string.Empty;
        return false;
    }

    /// <summary>
    /// Envoie une requête par producteur de la destination donnée (colonne producteur, valeurs séparées par '|').
    /// Retourne un résultat par producteur.
    /// </summary>
    public async Task<IReadOnlyList<SendResult>> SendAsync(RmqDestination destination, CancellationToken cancellationToken = default)
    {
        var results = new List<SendResult>();
        var producers = destination.Producer.Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (producers.Length == 0)
        {
            results.Add(new SendResult(destination, null, null, false, 0, "Aucun producteur renseigné dans le CSV", null));
            return results;
        }

        foreach (var producer in producers)
        {
            if (!TryResolveKeyId(producer, out var keyId))
            {
                results.Add(new SendResult(destination, producer, null, false, 0, $"Producteur inconnu (pas de KeyId) : {producer}", null));
                continue;
            }

            // Une requête par producteur : le KeyId est porté par la requête (et non par le client partagé),
            // ce qui reste correct quand plusieurs destinations sont envoyées en parallèle.
            var request = new RestRequest(SendResource, Method.Post);
            request.AddHeader("KeyId", keyId);
            request.AddHeader("DestinationRmq", destination.Name);
            request.AddStringBody(_payloadProvider(destination), DataFormat.Json);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(_timeout);

            var sw = Stopwatch.StartNew();
            try
            {
                var response = await _client.ExecuteAsync(request, cts.Token).ConfigureAwait(false);
                sw.Stop();

                var error = response.IsSuccessful
                    ? null
                    : response.ErrorMessage ?? response.ErrorException?.Message ?? response.StatusDescription;

                results.Add(new SendResult(destination, producer,
                    response.StatusCode == 0 ? null : response.StatusCode,
                    response.IsSuccessful, sw.ElapsedMilliseconds, error, response.Content));
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                sw.Stop();
                results.Add(new SendResult(destination, producer, null, false, sw.ElapsedMilliseconds,
                    $"Timeout après {_timeout.TotalSeconds:0}s", null));
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                sw.Stop();
                results.Add(new SendResult(destination, producer, null, false, sw.ElapsedMilliseconds, ex.Message, null));
            }
        }

        return results;
    }

    /// <summary>
    /// Envoie les requêtes pour chacune des destinations et retourne la liste à plat des résultats
    /// (un résultat par destination et par producteur), dans l'ordre des destinations.
    /// </summary>
    /// <param name="maxParallelism">1 = envois séquentiels (par défaut).</param>
    public async Task<IReadOnlyList<SendResult>> RunAsync(
        IEnumerable<RmqDestination> destinations,
        int maxParallelism = 1,
        CancellationToken cancellationToken = default)
    {
        var list = destinations.ToList();
        // Un emplacement par destination : pas d'écriture concurrente sur une même liste.
        var perDestination = new IReadOnlyList<SendResult>[list.Count];
        using var gate = new SemaphoreSlim(Math.Max(1, maxParallelism));
        var done = 0;

        var tasks = list.Select(async (destination, index) =>
        {
            await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                var destinationResults = await SendAsync(destination, cancellationToken).ConfigureAwait(false);
                perDestination[index] = destinationResults;

                var n = Interlocked.Increment(ref done);
                foreach (var r in destinationResults)
                {
                    var status = r.Success ? "OK " : "KO ";
                    var code = r.StatusCode.HasValue ? ((int)r.StatusCode.Value).ToString() : "---";
                    _log($"[{n}/{list.Count}] {status} {code} {r.ElapsedMs,5} ms  {destination.Name} ({r.Producer})"
                         + (r.Success ? string.Empty : $"  -> {r.Error}"));
                }
            }
            finally
            {
                gate.Release();
            }
        });

        await Task.WhenAll(tasks).ConfigureAwait(false);
        return perDestination.SelectMany(r => r).ToList();
    }

    /// <summary>Affiche le récapitulatif et retourne le nombre d'échecs.</summary>
    public int PrintSummary(IReadOnlyList<SendResult> results)
    {
        var ko = results.Where(r => !r.Success).ToList();
        _log(string.Empty);
        _log($"Total : {results.Count} requêtes  |  OK : {results.Count - ko.Count}  |  KO : {ko.Count}");
        foreach (var r in ko)
        {
            var code = r.StatusCode.HasValue ? ((int)r.StatusCode.Value).ToString() : "---";
            _log($"  KO {code}  {r.Destination.Name} ({r.Producer})  {r.Error}");
        }

        return ko.Count;
    }

    /// <summary>Écrit un rapport CSV (séparateur ';', UTF-8 avec BOM pour Excel).</summary>
    public static void WriteReport(IEnumerable<SendResult> results, string path)
    {
        static string Esc(string? v)
        {
            v ??= string.Empty;
            v = v.Replace("\r", " ").Replace("\n", " ");
            return v.Contains(';') || v.Contains('"') ? $"\"{v.Replace("\"", "\"\"")}\"" : v;
        }

        var sb = new StringBuilder();
        sb.AppendLine("destination;type;producteur;exchange;routing_key;statut_http;succes;duree_ms;erreur;reponse");
        foreach (var r in results)
        {
            var d = r.Destination;
            sb.AppendLine(string.Join(';',
                Esc(d.Name), d.Kind, Esc(r.Producer), Esc(d.Exchange), Esc(d.RoutingKey),
                r.StatusCode.HasValue ? (int)r.StatusCode.Value : string.Empty,
                r.Success ? "OK" : "KO", r.ElapsedMs, Esc(r.Error), Esc(r.ResponseBody)));
        }

        File.WriteAllText(path, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
    }

    public void Dispose() => _client.Dispose();
}

public class TokenInfo
{
    public string access_token { get; set; }
}