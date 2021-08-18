using Xunit;
using CSGOMarketplace.Areas.Admin.Controllers;
using MyTested.AspNetCore.Mvc;
using static CSGOMarketplace.Areas.Admin.AdminConstants;
using static CSGOMarketplace.Tests.Data.Feedbacks;

namespace CSGOMarketplace.Tests.Controllers.Admin
{
    public class AdminFeedbackControllerTests
    {
        [Fact]
        public void GetAllFeedbackShouldReturnViewWhenAccessedWithAdmin()
            => MyController<FeedbackController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(x => x.AllFeedback())
                .ShouldReturn()
                .View();

        [Fact]
        public void DeleteFeedbackShouldReturnRedirectWhenDeletedCorrectly()
            => MyController<FeedbackController>
                .Instance(controller => controller
                    .WithData(TestFeedback)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(x => x.Delete(1))
                .ShouldReturn()
                .Redirect();

    }
}
