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

        [Required]
        public double Float { get; set; }

        public int ConditionId { get; set; }

        [Required]
        public Condition Condition { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public bool IsSold { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser Owner { get; set; }
    }
}
