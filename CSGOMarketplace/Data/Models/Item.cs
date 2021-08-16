using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CSGOMarketplace.Data.DataConstants;

namespace CSGOMarketplace.Data.Models
{
    public class Item
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(MaxItemNameLength)]
        public string Name { get; set; }

        public double? Float { get; set; }

        public int? ConditionId { get; set; }

        public Condition Condition { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string InspectUrl { get; set; }

        public decimal Price { get; set; }

        public bool IsSoldOrPendingSale { get; set; }

        public string UserId { get; set; }

        public User Owner { get; set; }
    }
}
