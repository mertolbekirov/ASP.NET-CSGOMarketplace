using System;
using System.Collections.Generic;
using System.Linq;
using CSGOMarketplace.Services.Items;
using CSGOMarketplace.Services.Items.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static CSGOMarketplace.WebConstants.Cache;

namespace CSGOMarketplace.Controllers
{
    public class HomeController : Controller
    {

        private readonly IItemService items;
        private readonly IMemoryCache cache;

        public HomeController(IItemService items, IMemoryCache cache)
        {
            this.items = items;
            this.cache = cache;
        }


        public IActionResult Index()
        {
            var latestItems = this.cache.Get<List<LatestItemServiceModel>>(LatestItemsCacheKey);

            if (latestItems == null)
            {
                latestItems = this.items
                    .Latest()
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(LatestItemsCacheKey, latestItems, cacheOptions);
            }

            return View(latestItems);
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}
