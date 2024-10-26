using AutoMapper;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

namespace CarRentalAPI.Application.Mapping
{
    public class ClosedCarOrderProfile : Profile
    {
        public ClosedCarOrderProfile() 
        {
            CreateMap<CarOrder, ClosedCarReservationResponse>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.CarId, opt => opt.MapFrom(src => src.CarId))
                 .ForMember(d => d.CarName, opt => opt.MapFrom(src => src.Car.Brand + "/" +
                 src.Car.Model))
                .ForMember(d => d.StartOfLease, opt => opt.MapFrom(src => src.StartOfLease))
                .ForMember(d => d.EndOfLease, opt => opt.MapFrom(src => src.EndOfLease))
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(d => d.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(d => d.CarImageURI, opt => opt.MapFrom(src => src.Car.CarImageURI))
                .ForMember(d => d.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(d => d.CarharingUserId, opt => opt.MapFrom(src => src.CarsharingUserId));
        }
    }
}
    