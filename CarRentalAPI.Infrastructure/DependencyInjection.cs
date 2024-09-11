﻿using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;


namespace CarRentalAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddPersistence();
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<Context.Context, PostgresContext>();
            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IJWTProvider, JWTProvider>();

            return services;
        }
    }
}