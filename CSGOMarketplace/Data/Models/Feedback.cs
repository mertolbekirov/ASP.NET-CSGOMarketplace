using System.ComponentModel.DataAnnotations;
using static CSGOMarketplace.Data.DataConstants;

namespace CSGOMarketplace.Data.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxTitleFeedbackLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(MaxBodyFeedbackLength)]
        public string Description { get; set; }

        [Required]
        public string UserId { get; init; }

        public User Creator { get; init; }
    }
}
