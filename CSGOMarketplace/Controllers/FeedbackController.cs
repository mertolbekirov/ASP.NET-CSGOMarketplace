using CSGOMarketplace.Infrastructure;
using CSGOMarketplace.Models.Feedback;
using CSGOMarketplace.Services.Feedback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static CSGOMarketplace.WebConstants;

namespace CSGOMarketplace.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService feedback;

        public FeedbackController(IFeedbackService feedback)
        {
            this.feedback = feedback;
        }

        [Authorize]
        public IActionResult GiveFeedback()
            => View();

        [HttpPost]
        [Authorize]
        public IActionResult GiveFeedback(FeedbackFormModel feedback)
        {
            if (!ModelState.IsValid)
            {
                return View(feedback);
            }

            this.feedback.Give(feedback.Title, feedback.Description, this.User.Id());

            TempData[GlobalMessageKey] = "Thank you for the feedback!";

            return RedirectToAction("All", "Items");
        }
    }
}
