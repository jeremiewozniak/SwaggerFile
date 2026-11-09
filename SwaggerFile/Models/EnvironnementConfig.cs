using System.Text.Json.Serialization;

namespace SwaggerFile.Models
{
    public class EnvironnementConfig
    {
        public Uri RmqUri { get; set; }
        public string RmqLogin { get; set; }
        public string RmqPassword { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Environnement
    {
        Dev,
        Rec,
        Ppr,
        Prod
    }
}
