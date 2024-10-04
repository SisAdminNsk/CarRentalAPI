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

        public async Task<ErrorOr<Success>> CloseAllOutdatedOpenedCarReservatiosAsync()
        {
            try
            {
                var outdatedOpenedCarOrders = await _context.CarOrders.
                    Where(order => order.Status == CarOrdersStatus.Opened)
                    .Where(order => order.EndOfLease <= DateTime.UtcNow).
                    ToListAsync();

               outdatedOpenedCarOrders.ForEach(order => order.Status = CarOrdersStatus.OutOfTime);

                await _context.SaveChangesAsync();

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
                var openCarOrder = await _context.CarOrders.
                    Where(co => co.Id == closeCarOrderRequest.OpenedCarOrderId).
                    FirstOrDefaultAsync();

                if(openCarOrder is null)
                {
                    return Error.NotFound("OpenCarOrder.NotFound", $"OpenCarOrder with GUID: " +
                        $"{closeCarOrderRequest.OpenedCarOrderId} was not found in database.");
                }

                openCarOrder.Status = CarOrdersStatus.Closed;

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
                // заменить startOfLease и endOfLease параметром LeaseDateTime
                var carOrder = new CarOrder
                (
                    carOrderRequest.LeaseDateTime.StartOfLease,
                    carOrderRequest.LeaseDateTime.EndOfLease,
                    carsharingUser,
                    car, 
                    carOrderRequest.Comment,
                    carOrderRequest.ApproximatePrice,
                    CarOrdersStatus.NotConsidered
                );

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
                var closedStatus = new HashSet<string>() { CarOrdersStatus.OutOfTime, CarOrdersStatus.Closed };

                var closedCarOrders = await _context.CarOrders.
                    AsNoTracking().
                    Where(co => closedStatus.Contains(co.Status)).
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
                var openedCarOrders = await _context.CarOrders.
                    AsNoTracking().
                    Where(co => co.Status == CarOrdersStatus.Opened).
                    AsNoTracking().
                    Include(co => co.Car).
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
                var closedStatus = new HashSet<string>() { CarOrdersStatus.OutOfTime, CarOrdersStatus.Closed };

                var closedCarOrders = await _context.CarOrders.
                    AsNoTracking().
                    Where(co => co.CarsharingUserId == carsharingUserId).
                    AsNoTracking().
                    Where(co => closedStatus.Contains(co.Status)).
                    AsNoTracking().
                    ToListAsync();

                return _mapper.Map<List<ClosedCarReservationResponse>>(closedCarOrders);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetClosedCarOrdersByCarsharingUserIdAsync.Failure", description: ex.Message);
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

                if(carOrder.Status != CarOrdersStatus.NotConsidered)
                {
                    return Error.Conflict("CarOrder.Conflict", $"CarOrder with GUID: {openCarReservationRequest.CarOrderId}" +
                        $" has status: {carOrder.Status}, but " +
                        $"expected: {CarOrdersStatus.NotConsidered}");
                }

                var errorOrCarStatus = await IsCarFreeForBooking(carOrder.CarId);

                if (errorOrCarStatus.IsError)
                {
                    return errorOrCarStatus.Errors;
                }

                if (!errorOrCarStatus.Value.IsFreeForBooking)
                {
                    return Error.Conflict("CarIsBusy.Conflict", $"Car with GUID: {carOrder.CarId} is busy for booking now.");
                }

                carOrder.StartOfLease = openCarReservationRequest.StartOfLease;
                carOrder.EndOfLease = openCarReservationRequest.EndOfLease;
                carOrder.Price = openCarReservationRequest.Price;

                carOrder.Status = CarOrdersStatus.Opened;

                await _context.SaveChangesAsync();

                return openCarReservationRequest;
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.OpenCarReservationAsync.Failure", description: ex.Message);
            }
        }
        public async Task<ErrorOr<List<OpenedCarReservationResponse>>> GetOpenedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId)
        {
            try
            {
                var openedCarOrders = await _context.CarOrders.
                    AsNoTracking().
                    Where(co => co.CarsharingUserId == carsharingUserId).
                    AsNoTracking()
                    .Where(co => co.Status == CarOrdersStatus.Opened).
                    AsNoTracking().
                    Include(co => co.Car).
                    AsNoTracking().
                    ToListAsync();

                return _mapper.Map<List<OpenedCarReservationResponse>>(openedCarOrders);
            }
            catch (Exception ex)
            {
                return Error.Failure("CarBookingService.GetOpenedCarOrdersByCarsharingUserIdAsync.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<List<CarOrderResponse>>> GetAllNotConsideredCarOrders()
        {
            try
            {
                var notConsideredCarOrders = await _context.CarOrders.
                    AsNoTracking().
                    Where(co => co.Status == CarOrdersStatus.NotConsidered).
                    AsNoTracking().
                    Include(co => co.CarsharingUser).
                    AsNoTracking().
                    ToListAsync();

                return _mapper.Map<List<CarOrderResponse>>(notConsideredCarOrders);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetAllNotConsideredCarOrders.Failure", description: ex.Message);
            }
        }

        public async Task<ErrorOr<CarStatusResponse>> IsCarFreeForBooking(Guid carId)
        {
            try
            {
                var isCarBusyNow = await _context.CarOrders.
                    AsNoTracking().
                    Where(co => co.CarId == carId).
                    AsNoTracking().
                    Where(co => co.Status == CarOrdersStatus.Opened).
                    AsNoTracking().
                    FirstOrDefaultAsync();

                if (isCarBusyNow is not null)
                {
                    return new CarStatusResponse()
                    {
                        BookerId = isCarBusyNow.CarsharingUserId,
                        IsFreeForBooking = false,
                        DeadlineBooking = isCarBusyNow.EndOfLease
                    };
                }

                return new CarStatusResponse() { IsFreeForBooking = true };
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.IsCarFreeForBooking.Failure", description: ex.Message);
            }      
        }
    }
}
