using CSGOMarketplace.Data;
using System.ComponentModel.DataAnnotations;

namespace CSGOMarketplace.Models.Items
{
    public class ItemFormModel
    {
        [Required]
        public string Name { get; init; }

        public double Float { get; init; }

        [Range(double.Epsilon, DataConstants.MaxItemPrice, ErrorMessage = "Please enter a value between 0 and 100000")]
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
