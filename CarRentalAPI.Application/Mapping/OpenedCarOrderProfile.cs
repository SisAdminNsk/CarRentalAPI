using AutoMapper;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

namespace CarRentalAPI.Application.Mapping
{
    public class OpenedCarOrderProfile : Profile
    {
        public OpenedCarOrderProfile() 
        {
            CreateMap<OpenCarOrder, OpenedCarReservationResponse>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.CarId, opt => opt.MapFrom(src => src.CarId))
                 .ForMember(d => d.CarName, opt => opt.MapFrom(src => src.Car.Brand + "/" +
                 src.CarOrder.Car.Model))

                 .ForMember(d => d.CarImageUri, opt => opt.MapFrom(src => src.Car.CarImageURI))
                 .ForMember(d => d.RentalTimeRemainInHours, opt => opt.MapFrom(src => (src.CarOrder.EndOfLease -
                 src.CarOrder.StartOfLease).Hours));
        }
    }
}
