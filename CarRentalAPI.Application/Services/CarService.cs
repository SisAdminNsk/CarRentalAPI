using AutoMapper;
using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
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

        public async Task<ErrorOr<List<CarDTO>>> GetCarsByBrandAsync(string brand)
        {
            try 
            {
                var groupedByBrandCars = await _context.Cars.Where(c => c.Brand == brand).ToListAsync();

                return _mapper.Map<List<CarDTO>>(groupedByBrandCars); 
            }
            catch (Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "CarService.Get.Failure",
                    "Error occured while getting cars by brand", metadata);
            }
        }

        public async Task<ErrorOr<List<CarDTO>>> GetCarsByClassAsync(string carClass)
        {
            try
            {
                var groupedByClassCars = await _context.Cars.Where(c => c.CarClass == carClass).ToListAsync();

                return _mapper.Map<List<CarDTO>>(groupedByClassCars);
            }
            catch (Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "CarService.Get.Failure",
                    "Error occured while getting cars by class", metadata);
            }
        }

        public async Task<ErrorOr<List<CarDTO>>> ShowAllAvailableCarsAsync()
        {
            try
            {
                // потом таким же запросом чистить старые записи
                var busyCarsGuids = _context.CarOrders.
                    Where(order => order.EndOfLease > DateTime.UtcNow)
                    .Select(order => order.Car.Id).ToHashSet();
                
                var freeForBookingCars = await _context.Cars.Where(car => !busyCarsGuids.Contains(car.Id)).ToListAsync();

                return _mapper.Map<List<CarDTO>>(freeForBookingCars);
            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "CarService.GetFreeCars.Failure",
                    "Error occured while getting free cars", metadata);
            }
        }

        public async Task<ErrorOr<List<CarDTO>>> ShowAllCarsAsync()
        {
            try
            {
                var cars =  await _context.Cars.ToListAsync();

                return _mapper.Map<List<CarDTO>>(cars);
            }
            catch (Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "CarService.Get.Failure",
                    "Error occured while getting all cars", metadata);
            }
        }
    }
}
