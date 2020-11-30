using System.Text.Json.Serialization;

namespace WebAPI.Models {
    public class Repository
        {
            [JsonPropertyName("name")]
            public string name { get; set; }

            [JsonPropertyName("description")]
            public string description { get; set; }
        }
    
    
}