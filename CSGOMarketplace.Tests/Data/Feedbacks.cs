using MyTested.AspNetCore.Mvc;
using CSGOMarketplace.Data.Models;

namespace CSGOMarketplace.Tests.Data
{
    class Feedbacks
    {
        public static Feedback TestFeedback
            => new Feedback
            {
                 Title = "Test feedback",
                 Description = "Test feedback",
                 UserId = TestUser.Identifier
            };
    }
}
