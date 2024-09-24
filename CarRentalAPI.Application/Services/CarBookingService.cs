using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
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

        public async Task<ErrorOr<Success>> CloseAllOutdatedOpenedCarReservatiosAsync(string status = "OutOfTime")
        {
            try
            {
                var outdatedOpenedCarReservations = await _context.OpenCarOrders.Include(reservation => reservation.CarOrder).
                    Where(reservation => reservation.CarOrder.EndOfLease >= DateTime.UtcNow).ToListAsync();

                List<ClosedCarOrder> closedCarOrders = new();

                foreach (var carOrder in outdatedOpenedCarReservations.Select(co => co.CarOrder))
                {
                    closedCarOrders.Add(new ClosedCarOrder(carOrder, status));
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.RemoveRange(outdatedOpenedCarReservations);
                    await _context.SaveChangesAsync();

                    await _context.AddRangeAsync(closedCarOrders);
                    await _context.SaveChangesAsync();
                }

                return Result.Success;

            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.CloseAllOutdatedOpenedCarReservatiosAsync.Failure",
                    description: ex.Message);
            }
        }

        public async Task<ErrorOr<ClosedCarOrder>> CloseCarReservationAsync(Guid openCarOrderId, string status)
        {
            try
            {
                var openCarOrder = await _context.OpenCarOrders.Include(oco => oco.CarOrder).AsNoTracking().
                    Where(oco => oco.Id == openCarOrderId).AsNoTracking().FirstOrDefaultAsync();

                if(openCarOrder is null)
                {
                    return Error.NotFound("OpenCarOrder.NotFound", $"OpenCarOrder with GUID: {openCarOrderId} " +
                        $"was not found in database.");
                }

                var closedCarOrder = new ClosedCarOrder(openCarOrder.CarOrder, status);

                _context.OpenCarOrders.Remove(openCarOrder);
                await _context.SaveChangesAsync();

                return closedCarOrder;
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.CloseCarReservationAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<CarOrderRequest>> CreateCarOrderAsync(CarOrderRequest carOrderRequest)
        {
            try
            {
                var car = await _context.Cars.FindAsync(carOrderRequest.CarId);

                if(car is null)
                {
                    return Error.NotFound("Car.NotFound", $"Car with GUID: {carOrderRequest.CarId} was not found in database.");
                }

                var carsharingUser = await _context.CarsharingUsers.
                    Where(u => u.Id == carOrderRequest.CarsharingUserId).FirstOrDefaultAsync();

                if(carsharingUser is null)
                {
                    return Error.NotFound("CarsharingUser.NotFound",
                        $"CarsharingUser with GUID: {carOrderRequest.CarsharingUserId} was not found " +
                        $"in database.");
                }

                // цена нуждается в повторном перерасчете на сервере (для безопасности и целостности данных)

                var carOrder = new CarOrder(carOrderRequest.StartOfLease, carOrderRequest.EndOfLease, carsharingUser, car, 
                    carOrderRequest.Comment, carOrderRequest.ApproximatePrice);

                await _context.CarOrders.AddAsync(carOrder);
                await _context.SaveChangesAsync();

                return carOrderRequest;

            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.CreateCarOrderAsync.Failure", description: ex.Message);
            }
        }

        public async  Task<ErrorOr<Deleted>> DeleteCarOrderAsync(Guid carOrderId)
        {
            try
            {
                var carOrder = await _context.CarOrders.FindAsync(carOrderId);

                if(carOrder is null)
                {
                    return Error.NotFound("CarOrder.NotFound", $"CarOrder with GUID: {carOrderId} was not found in database.");
                }

                _context.CarOrders.Remove(carOrder);

                await _context.SaveChangesAsync();

                return Result.Deleted;
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.DeleteCarOrderAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<List<CarOrder>>> GetAllCarOrdersAsync()
        {
            try
            {
                return await _context.CarOrders.ToListAsync();
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetAllCarOrdersAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<List<ClosedCarOrder>>> GetAllClosedCarOrdersAsync()
        {
            try
            {
                return await _context.ClosedCarOrders.ToListAsync();
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetAllClosedCarOrdersAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<List<OpenCarOrder>>> GetAllOpenedCarOrdersAsync()
        {
            try
            {
                return await _context.OpenCarOrders.ToListAsync();
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetAllOpenedCarOrdersAsync.Failure", description: ex.Message);
            }
        }

        public Task<ErrorOr<List<CarOrder>>> GetCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorOr<List<ClosedCarOrder>>> GetClosedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorOr<List<OpenCarOrder>>> GetOpenedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<ErrorOr<OpenCarOrder>> OpenCarReservationAsync(Guid carOrderId, DateTime startOfLease, DateTime endOfLease)
        {
            try
            {
                var carOrder = await _context.CarOrders.FindAsync(carOrderId);

                if(carOrder is null)
                {
                    return Error.NotFound("CarOrder.NotFound", $"CarOrder with GUID: {carOrderId} was not found in database.");
                }


                if (! await IsCarFreeForBooking(carOrder.CarId))
                {
                    return Error.Conflict("CarIsBusy.Conflict", $"Car with GUID: {carOrder.CarId} is busy for booking now.");
                }

                carOrder.StartOfLease = startOfLease;
                carOrder.EndOfLease = endOfLease;

                var openCarReservation = new OpenCarOrder(carOrder);
                 
                await _context.OpenCarOrders.AddAsync(openCarReservation);
                await _context.SaveChangesAsync();

                return openCarReservation;

            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.OpenCarReservationAsync.Failure", description: ex.Message);
            }
        }

        private async Task<bool> IsCarFreeForBooking(Guid carId)
        {
            return await _context.CarOrders.AnyAsync(co => co.CarId == carId);
        }
    }
}
