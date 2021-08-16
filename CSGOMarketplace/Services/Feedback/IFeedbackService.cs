using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOMarketplace.Services.Feedback
{
    public interface IFeedbackService
    {
        public bool Give(string userId);
    }
}
