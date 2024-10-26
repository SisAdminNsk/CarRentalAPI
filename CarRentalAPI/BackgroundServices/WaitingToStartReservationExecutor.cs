using CarRentalAPI.Application.Interfaces;

namespace CarRentalAPI.BackgroundServices
{
    public class WaitingToStartReservationExecutor : BackgroundService 
    {
        private readonly IServiceProvider _serviceProvider;
        private int _minimalTaskCompletionDelayInMinutes = 1;

        public WaitingToStartReservationExecutor(IServiceProvider serviceProvider)
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

                     await carBookingService.OpenAllWaitingToStartCarReservationsAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(_minimalTaskCompletionDelayInMinutes), stoppingToken);
            }
        }
    }

}
