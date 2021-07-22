﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CSGOMarketplace.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Item> Items { get; init; } = new List<Item>();

    }
}
