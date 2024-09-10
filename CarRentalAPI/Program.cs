
using CarRentalAPI.Application.Extentions;
using CarRentalAPI.Application.Mapping;
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
            builder.Services.AddInfrastructure();
            builder.Services.AddApiAuthenticationAndAuthorization();
            builder.Services.AddSecurity();
            
            builder.Services.AddWeightControl();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(CarProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAllOrigins");
            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
