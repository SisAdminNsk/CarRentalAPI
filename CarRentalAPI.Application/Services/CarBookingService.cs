using AutoMapper;
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
        private readonly IMapper _mapper;

        public CarBookingService(Context.Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<ErrorOr<CloseCarReservationRequest>> CloseCarReservationAsync(CloseCarReservationRequest closeCarOrderRequest)
        {
            try
            {
                var openCarOrder = await _context.OpenCarOrders.Include(oco => oco.CarOrder).AsNoTracking().
                    Where(oco => oco.Id == closeCarOrderRequest.OpenedCarOrderId).AsNoTracking().FirstOrDefaultAsync();

                if(openCarOrder is null)
                {
                    return Error.NotFound("OpenCarOrder.NotFound", $"OpenCarOrder with GUID: " +
                        $"{closeCarOrderRequest.OpenedCarOrderId} was not found in database.");
                }

                var closedCarOrder = new ClosedCarOrder(openCarOrder.CarOrder, closeCarOrderRequest.Status);

                _context.OpenCarOrders.Remove(openCarOrder);
                await _context.SaveChangesAsync();

                return closeCarOrderRequest;
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

        public async Task<ErrorOr<Deleted>> DeleteCarOrderAsync(Guid carOrderId)
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

        public async Task<ErrorOr<List<CarOrderResponse>>> GetAllCarOrdersAsync()
        {
            try
            {
                var carOrders = await _context.CarOrders.
                    AsNoTracking().
                    Include(co => co.CarsharingUser).
                    AsNoTracking().
                    ToListAsync();

                return _mapper.Map<List<CarOrderResponse>>(carOrders);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetAllCarOrdersAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<List<ClosedCarReservationResponse>>> GetAllClosedCarOrdersAsync()
        {
            try
            {
                var closedCarOrders = await _context.ClosedCarOrders.
                    AsNoTracking().
                    ToListAsync();

                return _mapper.Map<List<ClosedCarReservationResponse>>(closedCarOrders);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetAllClosedCarOrdersAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<List<OpenedCarReservationResponse>>> GetAllOpenedCarOrdersAsync()
        {
            try
            {
                var openedCarOrders =  await _context.OpenCarOrders.
                    AsNoTracking().
                    ToListAsync();

                return _mapper.Map<List<OpenedCarReservationResponse>>(openedCarOrders);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetAllOpenedCarOrdersAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<List<CarOrderResponse>>> GetCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId)
        {
            try
            {
                var carOrders = await _context.CarOrders.
                    AsNoTracking().
                    Include(co => co.CarsharingUser).
                    AsNoTracking().
                    Where(co => co.CarsharingUserId == carsharingUserId)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<CarOrderResponse>>(carOrders);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetCarOrdersByCarsharingUserIdAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<List<ClosedCarReservationResponse>>> GetClosedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId)
        {
            try
            {
                var closedCarOrders = await _context.ClosedCarOrders.
                    AsNoTracking().
                    Include(cco => cco.CarOrder).
                    AsNoTracking().
                    Where(cco => cco.CarOrder.CarsharingUserId == carsharingUserId).
                    AsNoTracking().
                    ToListAsync();

                return _mapper.Map<List<ClosedCarReservationResponse>>(closedCarOrders);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetClosedCarOrdersByCarsharingUserIdAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<List<OpenedCarReservationResponse>>> GetOpenedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId)
        {
            try
            {
                var openedCarOrders = await _context.OpenCarOrders.
                    AsNoTracking()
                    .Include(oco => oco.CarOrder).
                    AsNoTracking().
                    Where(oco => oco.CarOrder.CarsharingUserId == carsharingUserId)
                    .AsNoTracking().
                    ToListAsync();

                return _mapper.Map<List<OpenedCarReservationResponse>>(openedCarOrders);    
            }
            catch (Exception ex)
            {
                return Error.Failure("CarBookingService.GetOpenedCarOrdersByCarsharingUserIdAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<OpenCarReservationRequest>> OpenCarReservationAsync(OpenCarReservationRequest openCarReservationRequest)
        {
            try
            {
                var carOrder = await _context.CarOrders.FindAsync(openCarReservationRequest.CarOrderId);

                if(carOrder is null)
                {
                    return Error.NotFound("CarOrder.NotFound", $"CarOrder with GUID: {openCarReservationRequest.CarOrderId} " +
                        $"was not found in database.");
                }


                if (! await IsCarFreeForBooking(carOrder.CarId))
                {
                    return Error.Conflict("CarIsBusy.Conflict", $"Car with GUID: {carOrder.CarId} is busy for booking now.");
                }

                carOrder.StartOfLease = openCarReservationRequest.StartOfLease;
                carOrder.EndOfLease = openCarReservationRequest.EndOfLease;
                carOrder.Price = openCarReservationRequest.Price;

                var openCarReservation = new OpenCarOrder(carOrder);
                 
                await _context.OpenCarOrders.AddAsync(openCarReservation);
                await _context.SaveChangesAsync();

                return openCarReservationRequest;

            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.OpenCarReservationAsync.Failure", description: ex.Message);
            }
        }

        public async Task<bool> IsCarFreeForBooking(Guid carId)
        {
            return await _context.OpenCarOrders.AnyAsync(co => co.CarId == carId);
        }
    }
}
