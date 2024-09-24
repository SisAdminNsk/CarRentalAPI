
using CarRentalAPI.Application.Extentions;
using CarRentalAPI.Application.Mapping;
using CarRentalAPI.BackgroundServices;
using CarRentalAPI.Infrastructure;

namespace CarRentalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AllowAllOrigins();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddInfrastructure();
            builder.Services.AddEmailVerification();
            builder.Services.AddApiAuthenticationAndAuthorization();
            builder.Services.AddSecurity();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddHostedService<OutdatedReservationsCleaner>();
            //builder.Services.AddSession();

            builder.Services.AddWeightControl();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(CarOrderProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAllOrigins");
            app.UseHttpsRedirection();
            app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}
