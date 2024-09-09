using CarRentalAPI.Contracts;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface ICarService
    {
        Task<ErrorOr<List<CarDTO>>> ShowAllCarsAsync();
        Task<ErrorOr<List<CarDTO>>> ShowAllAvailableCarsAsync();
        Task<ErrorOr<List<CarDTO>>> GetCarsByBrandAsync(string brand);
        Task<ErrorOr<List<CarDTO>>> GetCarsByClassAsync(string carClass);
    }
}
