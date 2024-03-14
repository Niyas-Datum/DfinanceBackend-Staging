using Newtonsoft.Json;

namespace Dfinance.LogViewer
{
    public class LogEntryModel
    {
        [JsonProperty("Timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("Level")]
        public string Level { get; set; }

        [JsonProperty("MessageTemplate")]
        public string MessageTemplate { get; set; }

        [JsonProperty("Exception")]
        public string Exception { get; set; }

        [JsonProperty("Properties")]
        public Dictionary<string, string> Properties { get; set; }
    }
}