using AutoMapper;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

namespace CarRentalAPI.Application.Mapping
{
    public class OpenedCarOrderProfile : Profile
    {
        public OpenedCarOrderProfile() 
        {
            CreateMap<CarOrder, OpenedCarReservationResponse>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.CarId, opt => opt.MapFrom(src => src.CarId))
                 .ForMember(d => d.CarName, opt => opt.MapFrom(src => src.Car.Brand + "/" +
                 src.Car.Model))

                 .ForMember(d => d.CarImageUri, opt => opt.MapFrom(src => src.Car.CarImageURI))
                 .ForMember(d => d.RentalTimeRemainInMinutes, opt => opt.MapFrom(src => CalcRemainTimeInMinutes(src.EndOfLease)));
        }

        private int CalcRemainTimeInMinutes(DateTime endOfLease)
        {
            if(endOfLease - DateTime.UtcNow >= TimeSpan.Zero) 
            {
                return (endOfLease - DateTime.UtcNow).Minutes;
            }

            return 0;
        }
    }
}
