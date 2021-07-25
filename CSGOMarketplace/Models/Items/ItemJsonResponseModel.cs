using Newtonsoft.Json;

namespace CSGOMarketplace.Models.Items
{
    public class ItemJsonResponseModel
    {
        [JsonProperty("floatvalue")]
        public string FloatValue { get; init; }
        
        public string ImageUrl { get; set; }
        [JsonProperty("item_name")]
        public string ItemName { get; set; }
        [JsonProperty("wear_name")]
        public string WearName { get; set; }
    }
}
