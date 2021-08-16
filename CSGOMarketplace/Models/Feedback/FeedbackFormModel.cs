using System.ComponentModel.DataAnnotations;
using static CSGOMarketplace.Data.DataConstants;

namespace CSGOMarketplace.Models.Feedback
{
    public class FeedbackFormModel
    {
        [Required]
        [StringLength(MaxTitleFeedbackLength, MinimumLength = MinTitleFeedbackLength)]
        public string Title { get; init; }

        [Required]
        [StringLength(MaxBodyFeedbackLength, MinimumLength = MinBodyFeedbackLength)]
        public string Description { get; init; }
    }
}
