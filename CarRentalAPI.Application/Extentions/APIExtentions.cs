using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalAPI.Application.Extentions
{
    public static class APIExtentions
    {
        public static void AddCarRental(this IServiceCollection services)
        {
            services.AddScoped<ICarBookingService, CarBookingService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IGarageManagmentService, GarageManagmentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICarsharingUsersService, CarsharingUsersService>();
        }
    }
}
