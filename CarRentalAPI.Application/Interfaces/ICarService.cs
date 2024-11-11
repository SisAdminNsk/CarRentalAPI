using CarRentalAPI.Contracts;
using CarRentalAPI.Core;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{    
    public interface ICarService
    {
        Task<ErrorOr<GetCarsResponse>> GetCars(PaginationsParamsRequest paginationParams);
        Task<ErrorOr<List<Car>>> GetAllCarsAsync();
        Task<ErrorOr<List<Car>>> GetAllAvailableCarsAsync();
        Task<ErrorOr<List<Car>>> GetCarsByBrandAsync(string brand);
        Task<ErrorOr<List<Car>>> GetCarsByClassAsync(string carClass);
        Task<ErrorOr<List<SearchCarResposne>>> SearchCarsByNameAsync(string carName);
    }
}
