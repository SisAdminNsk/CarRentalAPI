using CarRentalAPI.Contracts;
using CarRentalAPI.Core;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface ICarService
    {
        Task<ErrorOr<List<Car>>> ShowAllCarsAsync();
        Task<ErrorOr<List<Car>>> ShowAllAvailableCarsAsync();
        Task<ErrorOr<List<Car>>> GetCarsByBrandAsync(string brand);
        Task<ErrorOr<List<Car>>> GetCarsByClassAsync(string carClass);
    }
}
