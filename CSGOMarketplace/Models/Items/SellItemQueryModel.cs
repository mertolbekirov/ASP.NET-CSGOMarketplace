using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOMarketplace.Models.Items
{
    public class SellItemQueryModel
    {
        //Called S from steam - It's from SteamId
        public string S { get; init; }

        //Unknown parameter given from steam
        public string D{ get; init; }

        //Called A from steam - It's for asset ID
        public string A { get; init; }
    }
}
