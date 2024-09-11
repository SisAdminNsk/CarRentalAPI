using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

using ErrorOr;

namespace CarRentalAPI.Application.Services
{
    public class GarageManagmentService : IGarageManagmentService
    {
        private Context.Context _context;
        public GarageManagmentService(Context.Context context)
        {
            _context = context;
        }
        public async Task<ErrorOr<Created>> AddNewCarAsync(CarDTO car)
        {
            try
            {
                var carEntity = new Car(
                    car.Id,
                    car.Brand,
                    car.Model,
                    car.CarClass,
                    car.CarImageURI,
                    car.BaseRentalPricePerHour);

                await _context.Cars.AddAsync(carEntity);
                await _context.SaveChangesAsync();

                return Result.Created;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "GarageManagmentService.Add.Failure",
                    "Error occured while adding new car", metadata);
            }
        }

        public async Task<ErrorOr<Deleted>> DeleteCarAsync(Guid carId)
        {
            try
            {
                var carEntity = await _context.Cars.FindAsync(carId);

                _context.Cars.Remove(carEntity);
                await _context.SaveChangesAsync();

                return Result.Deleted;
            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "GarageManagmentService.Delete.Failure",
                    "Error occured while deleting car", metadata);
            }
        }

        public async Task<ErrorOr<Updated>> UpdateCarAsync(Guid carId, CarDTO car)
        {
            try
            {
                var carEntity = await _context.Cars.FindAsync(carId);

                carEntity.Brand = car.Brand;
                carEntity.CarClass = car.CarClass;
                carEntity.CarImageURI = car.CarImageURI;
                carEntity.Power = car.Power;
                carEntity.BaseRentalPricePerHour = car.BaseRentalPricePerHour;

                await _context.SaveChangesAsync();

                return Result.Updated;

            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "GarageManagmentService.Update.Failure",
                    "Error occured while updating car", metadata);
            }
        }
    }
}
