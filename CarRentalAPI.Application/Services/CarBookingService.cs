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

        public async Task<ErrorOr<CarOrderReply>> CreateOrUpdateCarOrderAsync(CarOrderRequest carOrderRequest)
        {
            try
            {
                var car = await _context.Cars.FindAsync(carOrderRequest.CarId);

                var carsharingUser = await _context.CarsharingUsers.
                    Where(u => u.Id == carOrderRequest.CarsharingUserId).
                    FirstOrDefaultAsync();

                if (car is null)
                {
                    return Error.NotFound("Car.NotFound", $"Car with GUID: {carOrderRequest.CarId} was not found in database.");
                }

                if (carsharingUser is null)
                {
                    return Error.NotFound("CarsharingUser.NotFound",
                        $"CarsharingUser with GUID: {carOrderRequest.CarsharingUserId} was not found " +
                        $"in database.");
                }

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

                var isCarsharingUserAlreadyHasCarOrder = await IsCarsharingUserAlreadyHasCarOrder(carsharingUser.Id, car.Id);

                if (!isCarsharingUserAlreadyHasCarOrder.IsError) 
                {
                    if (isCarsharingUserAlreadyHasCarOrder.Value is not null)
                    {
                        var existingCarOrder = isCarsharingUserAlreadyHasCarOrder.Value;

                        await UpdateCarOrderDetails(existingCarOrder, carOrder);

                        return new CarOrderReply(carOrderRequest, 204, "Updated");
                    }

                    await AddNewCarOrder(carOrder);

                    return new CarOrderReply(carOrderRequest, 200, "Created");
                }

                return isCarsharingUserAlreadyHasCarOrder.FirstError;               
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.CreateCarOrderAsync.Failure", description: ex.Message);
            }
        }

        private async Task UpdateCarOrderDetails(CarOrder existingcarOrder, CarOrder newCarOrderDetails)
        {
            existingcarOrder.Price = newCarOrderDetails.Price;
            existingcarOrder.Comment = newCarOrderDetails.Comment;
            existingcarOrder.StartOfLease = newCarOrderDetails.StartOfLease;
            existingcarOrder.EndOfLease = newCarOrderDetails.EndOfLease;

            await _context.SaveChangesAsync();
        }

        private async Task AddNewCarOrder(CarOrder newCarOrder)
        {
            await _context.CarOrders.AddAsync(newCarOrder);
            await _context.SaveChangesAsync();
        }

        private async Task<ErrorOr<CarOrder?>> IsCarsharingUserAlreadyHasCarOrder(Guid carsharingUserId, Guid carId)
        {
            try
            {
                var carOrder = await _context.CarOrders.
                    Where
                    (
                        co => co.CarsharingUserId == carsharingUserId 
                        && co.CarId == carId 
                        && co.Status == CarOrdersStatus.NotConsidered
                    )
                    .FirstOrDefaultAsync();

                return carOrder;
            }
            catch (Exception ex)
            {
                return Error.Failure("CarBookingService.IsCarsharingUserAlreadyHasCarOrder.Failure",
                    description: ex.Message);
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
                    Where(co => co.Status == CarOrdersStatus.Opened).
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
                    Include(co => co.CarsharingUser).
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
                    Where(co => co.CarsharingUserId == carsharingUserId).
                    Where(co => closedStatus.Contains(co.Status)).
                    Include(co => co.Car).
                    AsNoTracking().
                    OrderByDescending(co => co.StartOfLease).
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

                carOrder.StartOfLease = openCarReservationRequest.LeaseDateTime.StartOfLease;
                carOrder.EndOfLease = openCarReservationRequest.LeaseDateTime.EndOfLease;
                carOrder.Price = openCarReservationRequest.Price;
                carOrder.Status = CarOrdersStatus.WaitingToStart;

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
                await CloseAllOutdatedOpenedCarReserVationsOfCarhsaringUserAsync(carsharingUserId);
                await OpenAllWaitingToStartCarReservationsOfCarsharingUserAsync(carsharingUserId);

                var openedCarOrders = await _context.CarOrders.
                    Include(co => co.CarsharingUser).
                    Where(co => co.CarsharingUserId == carsharingUserId)
                    .Where(co => co.Status == CarOrdersStatus.Opened || co.Status == CarOrdersStatus.WaitingToStart).
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
                    Where(co => co.Status == CarOrdersStatus.NotConsidered).
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
                    Where(co => co.CarId == carId).
                    Where(co => co.Status == CarOrdersStatus.Opened || co.Status == CarOrdersStatus.WaitingToStart).
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

        public async Task<ErrorOr<OpenWaitingToStartReservationsResponse>> OpenAllWaitingToStartCarReservationsAsync()
        {
            try
            {
                var waitingTostartCarReservations = await _context.CarOrders.
                   Where(order => order.Status == CarOrdersStatus.WaitingToStart)
                   .Where(order => order.StartOfLease <= DateTime.UtcNow).
                   ToListAsync();

                waitingTostartCarReservations.ForEach(order => order.Status = CarOrdersStatus.Opened);

                await _context.SaveChangesAsync();

                return await GetOpenWaitingToStartReservationsResponse();

            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.OpenAllWaitingToStartCarReservationsAsync.Failure",
                    description: ex.Message);
            }
        }

        public async Task<ErrorOr<OpenWaitingToStartReservationsResponse>> OpenAllWaitingToStartCarReservationsOfCarsharingUserAsync(Guid carhsaringUserId)
        {
            try
            {
                var waitingToStartCarReservationsOfCarsharingUser = await _context.CarOrders.
                    Where(order => order.CarsharingUserId == carhsaringUserId).
                    Where(order => order.Status == CarOrdersStatus.WaitingToStart).
                    Where(order => order.StartOfLease <= DateTime.UtcNow).
                    ToListAsync();

                waitingToStartCarReservationsOfCarsharingUser.ForEach(order => order.Status = CarOrdersStatus.Opened);

                await _context.SaveChangesAsync();

                return await GetOpenWaitingToStartReservationsResponse();

            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.OpenAllWaitingToStartCarReservationsOfCarsharingUserAsync.Failure",
                  description: ex.Message);
            }
        }
        private async Task<ErrorOr<OpenWaitingToStartReservationsResponse>> GetOpenWaitingToStartReservationsResponse()
        {
            try
            {
                var nearestWaitingToStartCarReservation = await _context.CarOrders.
                    Where(order => order.Status == CarOrdersStatus.WaitingToStart).
                    OrderBy(order => Math.Abs((order.StartOfLease - DateTime.UtcNow).Ticks)).
                    AsNoTracking().
                    FirstOrDefaultAsync();

                if (nearestWaitingToStartCarReservation is not null)
                {
                    var timeToNextRequestInMinutes = (int)(nearestWaitingToStartCarReservation.StartOfLease - DateTime.UtcNow).TotalMinutes;

                    return new OpenWaitingToStartReservationsResponse(timeToNextRequestInMinutes, nearestWaitingToStartCarReservation.Id);
                }

                return new OpenWaitingToStartReservationsResponse(0, Guid.Empty, noOneRecord: true);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetOpenWaitingToStartReservationsResponse.Failure", description: ex.Message);
            }   
        }

        private async Task<ErrorOr<CloseOutdatedReservationsResponse>> GetCloseOutdatedReservationsResponse()
        {
            try
            {
                var nearestWaitingToCloseCarReservation = await _context.CarOrders.
                    Where(order => order.Status == CarOrdersStatus.WaitingToStart).
                    OrderBy(order => Math.Abs((order.EndOfLease - order.StartOfLease).Ticks)).
                    AsNoTracking().
                    FirstOrDefaultAsync();

                if (nearestWaitingToCloseCarReservation is not null)
                {
                    var timeToNextRequestInMinutes = (int)(nearestWaitingToCloseCarReservation.EndOfLease - nearestWaitingToCloseCarReservation.StartOfLease).TotalMinutes;

                    return new CloseOutdatedReservationsResponse(timeToNextRequestInMinutes, nearestWaitingToCloseCarReservation.Id);
                }

                return new CloseOutdatedReservationsResponse(0, Guid.Empty, noOneRecord: true);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.GetCloseOutdatedReservationsResponse.Failure", description: ex.Message);
            }          
        }

        public async Task<ErrorOr<CloseOutdatedReservationsResponse>> CloseAllOutdatedOpenedCarReserVationsOfCarhsaringUserAsync(Guid carsharingUserId)
        {
            try
            {
                var outdatedOpenedCarOrdersOfCarsharingUser = await _context.CarOrders.
                   Where(order => order.CarsharingUserId == carsharingUserId).
                   Where(order => order.Status == CarOrdersStatus.Opened)
                   .Where(order => order.EndOfLease <= DateTime.UtcNow).
                   ToListAsync();

                outdatedOpenedCarOrdersOfCarsharingUser.ForEach(order => order.Status = CarOrdersStatus.OutOfTime);

                await _context.SaveChangesAsync();

                return await GetCloseOutdatedReservationsResponse();
            }
            catch(Exception ex)
            {
                return Error.Failure("CarBookingService.CloseAllOutdatedOpenedCarReserVationsOfCarhsaringUserAsync.Failure",
                 description: ex.Message);
            }
        }

        public async Task<ErrorOr<CloseOutdatedReservationsResponse>> CloseAllOutdatedOpenedCarReservatiosAsync()
        {
            try
            {
                var outdatedOpenedCarOrders = await _context.CarOrders.
                    Where(order => order.Status == CarOrdersStatus.Opened)
                    .Where(order => order.EndOfLease <= DateTime.UtcNow).
                    ToListAsync();

                outdatedOpenedCarOrders.ForEach(order => order.Status = CarOrdersStatus.OutOfTime);

                await _context.SaveChangesAsync();

                return await GetCloseOutdatedReservationsResponse();

            }
            catch (Exception ex)
            {
                return Error.Failure("CarBookingService.CloseAllOutdatedOpenedCarReservatiosAsync.Failure",
                    description: ex.Message);
            }
        }
    }
}
