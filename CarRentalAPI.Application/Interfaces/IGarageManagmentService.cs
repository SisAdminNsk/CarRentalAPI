using CarRentalAPI.Contracts;
using CarRentalAPI.Core;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface IGarageManagmentService
    {
        Task<ErrorOr<Created>> AddNewCarAsync(Car car);
        Task<ErrorOr<Deleted>> DeleteCarAsync(Guid carId);
        Task<ErrorOr<Updated>> UpdateCarAsync(Guid carId, Car car);
    }
}
