using AutoMapper;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

namespace CarRentalAPI.Application.Mapping
{
    public class CarOrderProfile : Profile
    {
        public CarOrderProfile() 
        {
            CreateMap<CarOrder, CarOrderResponse>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(d => d.CarId, opt => opt.MapFrom(src => src.CarId))
              .ForMember(d => d.CarsharingUserId, opt => opt.MapFrom(src => src.CarsharingUserId))
             .ForMember(d => d.Price, opt => opt.MapFrom(src => src.Price))
             .ForMember(d => d.Surname, opt => opt.MapFrom(src => src.CarsharingUser.Surname))
             .ForMember(d => d.Name, opt => opt.MapFrom(src => src.CarsharingUser.Name))
             .ForMember(d => d.Patronymic, opt => opt.MapFrom(src => src.CarsharingUser.Patronymic))
             .ForMember(d => d.Phone, opt => opt.MapFrom(src => src.CarsharingUser.Phone))
             .ForMember(d => d.StartOfLease, opt => opt.MapFrom(src => src.StartOfLease))
             .ForMember(d => d.EndOfLease, opt => opt.MapFrom(src => src.EndOfLease))
             .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
