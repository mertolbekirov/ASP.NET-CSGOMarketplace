using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CSGOMarketplace.Data.DataConstants;

namespace CSGOMarketplace.Models.Items
{
    public class AddItemFormModel
    {
        [Required]
        public string Name { get; init; }

        [Range(0,1)]
        public double Float { get; init; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Display(Name = "Image URL")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        [Required]
        [Url]
        public string InspectUrl { get; set; }

        public string Condition { get; set; }
    }
}
