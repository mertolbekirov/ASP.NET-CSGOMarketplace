using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;

namespace CSGOMarketplace.Services.Feedback
{
    public class FeedbackService : IFeedbackService
    {
        private readonly MarketplaceDbContext data;

        public FeedbackService(MarketplaceDbContext data)
        {
            this.data = data;
        }

        public void Give(string title, string body, string userId)
        {
            var feedback = new Data.Models.Feedback
            {
                Title = title,
                Body = body,
                UserId = userId
            };


        }
    }
}
