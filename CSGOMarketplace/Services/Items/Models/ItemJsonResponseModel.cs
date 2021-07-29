using Newtonsoft.Json;

namespace CSGOMarketplace.Services.Items.Models
{
    public class ItemJsonResponseModel
    {
        [JsonProperty("floatvalue")]
        public double Float { get; init; }
        
        public string ImageUrl { get; init; }
        [JsonProperty("full_item_name")]
        public string Name { get; set; }

        [JsonProperty("wear_name")]
        public string ConditionName { get; init; }

    }
}
