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
                .ForMember(d => d.StartOfLease, opt => opt.MapFrom(src => src.StartOfLease))
                .ForMember(d => d.EndOfLease, opt => opt.MapFrom(src => src.EndOfLease))
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(d => d.CarharingUserId, opt => opt.MapFrom(src => src.CarsharingUserId));
        }
    }
}
    