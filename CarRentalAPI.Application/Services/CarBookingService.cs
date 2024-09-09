using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Core;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Application.Services
{
    public class CarBookingService : ICarBookingService
    {
        private Context.Context _context;

        public CarBookingService(Context.Context context)
        {
            _context = context;
        }

        public async Task<ErrorOr<Created>> BookCarAsync(Guid carId, Guid userId,  DateTime startOfLease, DateTime endOfLease)
        {
            try
            {
                var carEntity = await _context.Cars.FindAsync(carId);
                var carsharingUser = await _context.CarsharingUsers.Where(u => u.User.Id == userId).FirstOrDefaultAsync();

                var order = new CarOrder(startOfLease, endOfLease, carsharingUser, carEntity);

                await _context.CarOrders.AddAsync(order);
                await _context.SaveChangesAsync();

                return Result.Created;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "CarBookingService.Book.Failure", "Error occured while booking car", metadata);
            }
        }

        public async Task<ErrorOr<Deleted>> CancelCarReservationAsync(Guid orderId)
        {
            try
            {
                var orderEntity = await _context.CarOrders.FindAsync(orderId);

                _context.CarOrders.Remove(orderEntity);
                await _context.SaveChangesAsync();

                return Result.Deleted;
            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "CarBookingService.CancelReservation.Failure",
                    "Error occured while cancelling car reservation", metadata);
            }
        }
    }
}
