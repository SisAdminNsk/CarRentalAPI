using AutoMapper;

using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Application.Services
{
    public class CarService : ICarService
    {

        private Context.Context _context;
        private IMapper _mapper;

        public CarService(Context.Context context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ErrorOr<List<SearchCarResposne>>> SearchCarsByNameAsync(string carName)
        {
            throw new NotImplementedException();
        }

        public async Task<ErrorOr<List<Car>>> GetCarsByBrandAsync(string brand)
        {
            try 
            {
                var groupedByBrandCars = await _context.Cars.Where(c => c.Brand == brand).ToListAsync();

                return _mapper.Map<List<Car>>(groupedByBrandCars); 
            }
            catch (Exception ex)
            { 
                Dictionary<string, object> metadata = new() { { "Exception", ex.Message } };

                return Error.Failure(code: "CarService.GetCarsByBrandAsync.Failure",
                    "Error occured while getting cars by brand", metadata);
            }
        }

        public async Task<ErrorOr<List<Car>>> GetCarsByClassAsync(string carClass)
        {
            try
            {
                var groupedByClassCars = await _context.Cars.Where(c => c.CarClass == carClass).ToListAsync();

                return _mapper.Map<List<Car>>(groupedByClassCars);
            }
            catch (Exception ex)
            {
                Dictionary<string, object> metadata = new() { { "Exception", ex.Message } };

                return Error.Failure(code: "CarService.GetCarsByClassAsync.Failure",
                    "Error occured while getting cars by class", metadata);
            }
        }

        public async Task<ErrorOr<List<Car>>> GetAllAvailableCarsAsync()
        {
            try
            {
                var busyCarsGuids = _context.CarOrders.
                    Where(order => order.EndOfLease <= DateTime.UtcNow)
                    .Select(order => order.Car.Id).ToHashSet();
                
                var freeForBookingCars = await _context.Cars.Where(car => !busyCarsGuids.Contains(car.Id)).ToListAsync();

                return _mapper.Map<List<Car>>(freeForBookingCars);
            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new() { { "Exception", ex.Message } };

                return Error.Failure(code: "CarService.GetAllAvailableCarsAsync.Failure",
                    "Error occured while getting free cars", metadata);
            }
        }

        public async Task<ErrorOr<List<Car>>> GetAllCarsAsync()
        {
            try
            {
                var cars = await _context.Cars.
                    AsNoTracking().
                    ToListAsync();

                return _mapper.Map<List<Car>>(cars);
            }
            catch (Exception ex)
            {
                Dictionary<string, object> metadata = new() { { "Exception", ex.Message } };

                return Error.Failure(code: "CarService.GetAllCarsAsync.Failure",
                    "Error occured while getting all cars", metadata);
            }
        }

        public async Task<ErrorOr<GetCarsResponse>> GetCars(PaginationsParamsRequest paginationParams)
        {
            try
            {
                var query = _context.Cars.AsQueryable();

                if (paginationParams.SortOrder.ToLower() == SortOrdersPagination.Descending)
                {
                    query = query.OrderByDescending(e => EF.Property<object>(e, paginationParams.SortBy));
                }

                if (paginationParams.SortOrder.ToLower() == SortOrdersPagination.Ascending)
                {
                    query = query.OrderBy(e => EF.Property<object>(e, paginationParams.SortBy));
                }

                var totalItems = await query.CountAsync();

                var items = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).
                    Take(paginationParams.PageSize).ToListAsync();

                return new GetCarsResponse
                (
                    totalItems, 
                    paginationParams.PageNumber, 
                    paginationParams.PageSize,
                    items
                );
            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new() { { "Exception", ex.Message } };

                return Error.Failure(code: "CarService.GetCars.Failure",
                    "Error occured while getting paginated cars", metadata);
            }
        }
    }
}
