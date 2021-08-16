using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOMarketplace.Data.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string UserId { get; init; }

        public User Creator { get; init; }
    }
}
