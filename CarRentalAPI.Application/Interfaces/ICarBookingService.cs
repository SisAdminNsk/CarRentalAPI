using CarRentalAPI.Contracts;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface ICarBookingService
    {
        Task<ErrorOr<CarOrderRequest>> CreateCarOrderAsync(CarOrderRequest carOrderRequest);
        Task<ErrorOr<Deleted>> DeleteCarOrderAsync(Guid carOrderId);
        Task<ErrorOr<OpenCarReservationRequest>> OpenCarReservationAsync(OpenCarReservationRequest openCarReservationRequest);
        Task<ErrorOr<CloseCarReservationRequest>> CloseCarReservationAsync(CloseCarReservationRequest closeCarOrderRequest);
        Task<ErrorOr<Success>> CloseAllOutdatedOpenedCarReservatiosAsync(string status = "OutOfTime");
        Task<ErrorOr<List<CarOrderResponse>>> GetAllCarOrdersAsync();
        Task<ErrorOr<List<CarOrderResponse>>> GetCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId);
        Task<ErrorOr<List<OpenedCarReservationResponse>>> GetAllOpenedCarOrdersAsync();
        Task<ErrorOr<List<OpenedCarReservationResponse>>> GetOpenedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId);
        Task<ErrorOr<List<ClosedCarReservationResponse>>> GetAllClosedCarOrdersAsync();
        Task<ErrorOr<List<ClosedCarReservationResponse>>> GetClosedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId);
        Task<bool> IsCarFreeForBooking(Guid carId);
    }
}
