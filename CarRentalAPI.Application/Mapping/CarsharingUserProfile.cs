using AutoMapper;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

namespace CarRentalAPI.Application.Mapping
{
    public class CarsharingUserProfile : Profile
    {
        public CarsharingUserProfile() 
        {
            CreateMap<CarsharingUser, CarsharingUserResponse>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(d => d.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(d => d.Patronymic, opt => opt.MapFrom(src => src.Patronymic))
                .ForMember(d => d.Age, opt => opt.MapFrom(src => src.Age));
        }
    }
}
