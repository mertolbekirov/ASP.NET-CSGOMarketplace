using System.Collections.Generic;
using CSGOMarketplace.Services.Feedback.Models;

namespace CSGOMarketplace.Services.Feedback
{
    public interface IFeedbackService
    {
        public void Give(string title, string body, string userId);

        public IEnumerable<FeedbackServiceModel> GetAll();

        public FeedbackServiceModel GetById(int id);
    }
}
