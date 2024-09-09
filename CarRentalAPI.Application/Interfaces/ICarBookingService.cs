using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface ICarBookingService
    {
        Task<ErrorOr<Created>> BookCarAsync(Guid carId, Guid userId, DateTime startOfLease, DateTime endOfLease);
        Task<ErrorOr<Deleted>> CancelCarReservationAsync(Guid orderId);
    }
}
