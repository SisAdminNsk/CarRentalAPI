using CarRentalAPI.Contracts;
using CarRentalAPI.Core;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface ICarBookingService
    {
        Task<ErrorOr<CarOrderRequest>> CreateCarOrderAsync(CarOrderRequest carOrderRequest);
        Task<ErrorOr<Deleted>> DeleteCarOrderAsync(Guid carOrderId);
        Task<ErrorOr<OpenCarOrder>> OpenCarReservationAsync(Guid carOrderId, DateTime startOfLease, DateTime endOfLease);
        Task<ErrorOr<ClosedCarOrder>> CloseCarReservationAsync(Guid openCarOrderId, string status);
        Task<ErrorOr<Success>> CloseAllOutdatedOpenedCarReservatiosAsync(string status = "OutOfTime");
        Task<ErrorOr<List<CarOrder>>> GetAllCarOrdersAsync();
        Task<ErrorOr<List<CarOrder>>> GetCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId);
        Task<ErrorOr<List<OpenCarOrder>>> GetAllOpenedCarOrdersAsync();
        Task<ErrorOr<List<OpenCarOrder>>> GetOpenedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId);
        Task<ErrorOr<List<ClosedCarOrder>>> GetAllClosedCarOrdersAsync();
        Task<ErrorOr<List<ClosedCarOrder>>> GetClosedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId);
    }
}
