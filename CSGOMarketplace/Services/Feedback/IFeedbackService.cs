using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOMarketplace.Services.Feedback
{
    public interface IFeedbackService
    {
        public void Give(string title, string body, string userId);
    }
}
