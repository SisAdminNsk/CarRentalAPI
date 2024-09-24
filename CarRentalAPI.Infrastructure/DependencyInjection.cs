using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Application.Interfaces.Email;
using CarRentalAPI.Application.Services;
using CarRentalAPI.Infrastructure.Email;
using CarRentalAPI.Infrastructure.Email.Dependences;
using CarRentalAPI.Infrastructure.Security;
using CarRentalAPI.Infrastructure.Validation;
using Microsoft.Extensions.DependencyInjection;


namespace CarRentalAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddPersistence();
            services.AddTransient<IValidationService, ValidationService>();

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<Context.Context, PostgresContext>();

            services.AddScoped<ICarBookingService, CarBookingService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IGarageManagmentService, GarageManagmentService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IJWTProvider, JWTProvider>();

            return services;
        }

        public static IServiceCollection AddEmailVerification(this IServiceCollection services)
        {
            services.AddTransient<IEmailConfirmationService, EmailConfirmationService>();
            services.AddTransient<IVerificationCodeGenerator, Verification6DigitCodeGenerator>();
            services.AddTransient<ICodeVerificationService, CodeVerificationService>();
            services.AddTransient<IConcurrentVerificationCodesStorage, ConcurrentVerificationCodesStorage>();

            return services;
        }
    }
}
