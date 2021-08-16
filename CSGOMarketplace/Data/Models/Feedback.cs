using System.ComponentModel.DataAnnotations;
using AngleSharp.Common;
using static CSGOMarketplace.Data.DataConstants;

namespace CSGOMarketplace.Data.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [MaxLength(MaxTitleFeedbackLength, ErrorMessage = "You can't input more than 40 characters for the title.")]
        public string Title { get; set; }

        [MaxLength(MaxBodyFeedbackLength, ErrorMessage = "You can't input more than 400 characters for the body.")]
        public string Body { get; set; }

        [Required]
        public string UserId { get; init; }

        public User Creator { get; init; }
    }
}
