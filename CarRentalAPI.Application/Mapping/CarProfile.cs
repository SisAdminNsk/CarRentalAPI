using AutoMapper;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

namespace CarRentalAPI.Application.Mapping
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            //CreateMap<Car, CarDTO>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(d => d.Power, opt => opt.MapFrom(src => src.Power))
            //    .ForMember(d => d.CarClass, opt => opt.MapFrom(src => src.CarClass))
            //    .ForMember(d => d.BaseRentalPricePerHour, opt => opt.MapFrom(src => src.BaseRentalPricePerHour))
            //    .ForMember(d => d.Brand, opt => opt.MapFrom(src => src.Brand))
            //    .ForMember(d => d.CarImageURI, opt => opt.MapFrom(src => src.CarImageURI));
        }       
    }
}
