using CarRentalAPI.Application.Filters;
using CarRentalAPI.Application.Paginations;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{

    public interface ICarBookingService
    {
        Task<ErrorOr<CarOrderReply>> CreateOrUpdateCarOrderAsync(CarOrderRequest carOrderRequest);
        Task<ErrorOr<Deleted>> DeleteCarOrderAsync(Guid carOrderId);
        Task<ErrorOr<OpenCarReservationRequest>> OpenCarReservationAsync(OpenCarReservationRequest openCarReservationRequest);
        Task<ErrorOr<CloseCarReservationRequest>> CloseCarReservationAsync(CloseCarReservationRequest closeCarOrderRequest);
        Task<ErrorOr<CloseOutdatedReservationsResponse>> CloseAllOutdatedOpenedCarReservatiosAsync();
        Task<ErrorOr<CloseOutdatedReservationsResponse>> CloseAllOutdatedOpenedCarReserVationsOfCarhsaringUserAsync(Guid carsharingUserId);
        Task<ErrorOr<OpenWaitingToStartReservationsResponse>> OpenAllWaitingToStartCarReservationsAsync();
        Task<ErrorOr<OpenWaitingToStartReservationsResponse>> OpenAllWaitingToStartCarReservationsOfCarsharingUserAsync(Guid carsharingUserId);
        Task<ErrorOr<List<CarOrderResponse>>> GetAllCarOrdersAsync();
        Task<ErrorOr<List<CarOrderResponse>>> GetAllNotConsideredCarOrders();
        Task<ErrorOr<List<CarOrderResponse>>> GetCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId);
        Task<ErrorOr<List<OpenedCarReservationResponse>>> GetAllOpenedCarOrdersAsync();
        Task<ErrorOr<List<OpenedCarReservationResponse>>> GetOpenedCarOrdersByCarsharingUserIdAsync(Guid carsharingUserId);
        Task<ErrorOr<List<ClosedCarReservationResponse>>> GetAllClosedCarOrdersAsync();
        Task<ErrorOr<PaginatedClosedCarReservationsResponse>> GetClosedCarOrdersByCarsharingUserIdAsync(CarOrderFilter filter, SortParams sortParams, PageParams pageParams, Guid carsharingUserId);
        Task<ErrorOr<CarStatusResponse>> IsCarFreeForBooking(Guid carId);
    }
}
