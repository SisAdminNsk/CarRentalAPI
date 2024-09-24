using CarRentalAPI.Contracts;
using CarRentalAPI.Core;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface ICarBookingService
    {
        Task<ErrorOr<CreateCarOrderRequest>> CreateCarOrderAsync(CreateCarOrderRequest carOrderRequest);
        Task<ErrorOr<Deleted>> DeleteCarOrderAsync(Guid carOrderId);
        Task<ErrorOr<OpenCarOrder>> OpenCarReservationAsync(Guid carOrderId, DateTime startOfLease, DateTime endOfLease);
        Task<ErrorOr<ClosedCarOrder>> CloseCarReservationAsync(Guid openCarOrderId, string status);
    }
}
