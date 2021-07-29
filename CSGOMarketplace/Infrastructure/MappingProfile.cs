using AutoMapper;
using CSGOMarketplace.Models.Items;
using CSGOMarketplace.Services.Items.Models;

namespace CSGOMarketplace.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<ItemJsonResponseModel, ItemServiceModel>();

            this.CreateMap<ItemServiceModel, ItemFormModel>();
        }
    }
}
