using AutoMapper;
using Ecom_Onboarding.DTO;
using Ecom_Onboarding.Models;


namespace Ecom_Onboarding
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<GameDTO, Game>();
            CreateMap<Game, GameDTO>();
            CreateMap<Publisher, PublisherDTO>();
            CreateMap<PublisherDTO, Publisher>();
        }
    }
}
