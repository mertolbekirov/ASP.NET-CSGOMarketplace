using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSGOMarketplace.Models.Items
{
    public class ItemInfoJsonResponseModel
    {
        [JsonProperty("iteminfo")] public ItemJsonResponseModel ItemInfo { get; set; }
    }
}
