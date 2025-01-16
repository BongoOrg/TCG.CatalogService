using Newtonsoft.Json;

namespace TCG.CatalogService.Domain
{
    public class Extension
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Libelle { get; set; }

        [JsonProperty("symbol")]
        public string Symbole { get; set; }
        
        [JsonProperty("logo")]
        public string Logo { get; set; }
    }
}
