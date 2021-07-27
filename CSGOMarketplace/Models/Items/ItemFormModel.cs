using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CSGOMarketplace.Data.DataConstants;

namespace CSGOMarketplace.Models.Items
{
    public class ItemFormModel
    {
        [Required]
        public string Name { get; init; }

        public double Float { get; init; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; init; }

        [Display(Name = "Image URL")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        [Required]
        public string InspectUrl { get; init; }

        public string Condition { get; init; }
    }
}
