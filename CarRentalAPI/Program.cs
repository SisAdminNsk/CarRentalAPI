
using CarRentalAPI.Application.Extentions;
using CarRentalAPI.Application.Mapping;
using CarRentalAPI.BackgroundServices;
using CarRentalAPI.Infrastructure;
using System.Net.Sockets;
using System.Net;

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
            builder.Services.AddApiAuthenticationAndAuthorization(builder.Configuration);
            builder.Services.AddSecurity();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddHostedService<OutdatedReservationsCleaner>();
            builder.Services.AddHostedService<WaitingToStartReservationExecutor>();
            //builder.Services.AddSession();

            builder.Services.AddCarRental();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(CarOrderProfile));

            string LocalIp = LocalIPAddress();
            builder.WebHost.UseUrls("http://localhost:80", "http://" + LocalIp + ":5072");
                
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

            
            //app.Urls.Add("http://" + LocalIp + ":5072");
            //var hamachiAddress = "25.10.158.229";
            //app.Urls.Add("http://" + hamachiAddress + ":80");

            app.Run();
        }
        static string LocalIPAddress()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
                if (endPoint != null)
                {
                    return endPoint.Address.ToString();
                }
                else
                {
                    return "127.0.0.1";
                }
            }
        }

    }
}
