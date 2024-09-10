using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalAPI.Application.Extentions
{
    public static class APIExtentions
    {
        public static void AddWeightControl(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICarBookingService, CarBookingService>();
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<IGarageManagmentService, GarageManagmentService>();
        }
    }
}
