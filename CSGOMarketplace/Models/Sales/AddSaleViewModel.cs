using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CSGOMarketplace.Data.DataConstants;

namespace CSGOMarketplace.Models.Sales
{
    public class AddSaleViewModel
    {
            [Required]
        [StringLength(MaxItemNameLength, MinimumLength = MinItemNameLength)]
        public string Name { get; init; }

        [Range(0,1)]
        public double Float { get; init; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Display(Name = "Image URL")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        [Display(Name = "Condition")]
        public int ConditionId { get; init; }

        public IEnumerable<SaleConditionViewModel> Conditions { get; set; }
    }
}
