using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSGOMarketplace.Data;
using CSGOMarketplace.Services.Feedback.Models;

namespace CSGOMarketplace.Services.Feedback
{
    public class FeedbackService : IFeedbackService
    {
        private readonly MarketplaceDbContext data;
        private readonly IMapper mapper;

        public FeedbackService(MarketplaceDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public void Give(string title, string description, string userId)
        {
            var feedback = new Data.Models.Feedback
            {
                Title = title,
                Description = description,
                UserId = userId
            };

            this.data.Feedback.Add(feedback);
            this.data.SaveChanges();
        }

        public IEnumerable<FeedbackServiceModel> GetAll()
            => this.data
                .Feedback
                .OrderByDescending(x => x.Id)
                .ProjectTo<FeedbackServiceModel>(mapper.ConfigurationProvider)
                .ToList();

        public FeedbackServiceModel GetById(int id)
            => this.data
                .Feedback
                .Where(x => x.Id == id)
                .ProjectTo<FeedbackServiceModel>(mapper.ConfigurationProvider)
                .FirstOrDefault();

        public bool DeleteById(int id)
        {
            var feedback = this.data.Feedback.Find(id);

            if (feedback == null)
            {
                return false;
            }

            this.data.Feedback.Remove(feedback);
            this.data.SaveChanges();
            return true;

        }


    }
}
