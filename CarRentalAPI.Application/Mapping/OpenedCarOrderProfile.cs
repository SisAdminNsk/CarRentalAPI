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

                 .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status))
                 .ForMember(d => d.Comment, opt => opt.MapFrom(src => src.Comment))
                 .ForMember(d => d.StartOfLease, opt => opt.MapFrom(src => src.StartOfLease))
                 .ForMember(d => d.DeadlineDateTime, opt => opt.MapFrom(src => src.EndOfLease))
                 .ForMember(d => d.CarImageUri, opt => opt.MapFrom(src => src.Car.CarImageURI))
                 .ForMember(d => d.RentalTimeRemainInSeconds, opt => opt.MapFrom(src => CalcRemainTimeInSeconds(src.EndOfLease)));
        }

        private int CalcRemainTimeInSeconds(DateTime endOfLease)
        {
            if(endOfLease - DateTime.UtcNow >= TimeSpan.Zero) 
            {
                return (int)(endOfLease - DateTime.UtcNow).TotalSeconds;
            }

            return 0;
        }
    }
}
