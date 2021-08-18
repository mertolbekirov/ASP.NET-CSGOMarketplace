using MyTested.AspNetCore.Mvc;
using Xunit;
using CSGOMarketplace.Controllers;
using CSGOMarketplace.Models.Feedback;

namespace CSGOMarketplace.Tests.Controllers
{
    public class FeedbackControllerTest
    {
        [Fact]
        public void GetGiveFeedbackShouldReturnCorrectView()
            => MyController<FeedbackController>
                .Instance(controller => controller
                    .WithUser())
            .Calling(x => x.GiveFeedback())
            .ShouldHave()
            .ActionAttributes(attr => attr
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View();

        [Theory]
        [InlineData("Feedback Title", "Feedback Content")]
        public void PostGiveFeedbackShouldSaveFeedbackCorrectly(string title, string description)
            => MyController<FeedbackController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(x => x.GiveFeedback(new FeedbackFormModel
                {
                    Title = title,
                    Description = description
                }))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ItemsController>(c => c.All(null)));


    }   
}
