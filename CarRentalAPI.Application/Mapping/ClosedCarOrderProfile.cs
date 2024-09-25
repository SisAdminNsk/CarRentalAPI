using AutoMapper;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

namespace CarRentalAPI.Application.Mapping
{
    public class ClosedCarOrderProfile : Profile
    {
        public ClosedCarOrderProfile() 
        {
            CreateMap<ClosedCarOrder, ClosedCarReservationResponse>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(d => d.StartOfLease, opt => opt.MapFrom(src => src.CarOrder.StartOfLease))
                .ForMember(d => d.EndOfLease, opt => opt.MapFrom(src => src.CarOrder.EndOfLease))
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
