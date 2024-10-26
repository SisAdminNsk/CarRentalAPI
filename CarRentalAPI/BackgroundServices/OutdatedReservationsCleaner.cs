using CarRentalAPI.Application.Interfaces;

namespace CarRentalAPI.BackgroundServices
{
    public class OutdatedReservationsCleaner : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private int _minimalTaskCompletionDelayInMinutes = 1;

        public OutdatedReservationsCleaner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var carBookingService = scope.ServiceProvider.GetRequiredService<ICarBookingService>();

                    await carBookingService.CloseAllOutdatedOpenedCarReservatiosAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(_minimalTaskCompletionDelayInMinutes), stoppingToken);
            }            
        }
    }

}
