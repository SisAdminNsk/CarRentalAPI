using CarRentalAPI.Contracts;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface IGarageManagmentService
    {
        Task<ErrorOr<Created>> AddNewCarAsync(CarDTO car);
        Task<ErrorOr<Deleted>> DeleteCarAsync(Guid carId);
        Task<ErrorOr<Updated>> UpdateCarAsync(Guid carId, CarDTO car);
    }
}
