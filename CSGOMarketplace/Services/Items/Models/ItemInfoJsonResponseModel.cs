using CSGOMarketplace.Models.Items;
using Newtonsoft.Json;

namespace CSGOMarketplace.Services.Items.Models
{
    public class ItemInfoJsonResponseModel
    {
        [JsonProperty("iteminfo")] 
        public ItemJsonResponseModel ItemInfo { get; set; }
    }
}
