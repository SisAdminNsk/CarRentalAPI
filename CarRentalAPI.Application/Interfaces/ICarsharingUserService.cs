using CarRentalAPI.Contracts;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface ICarsharingUsersService
    {
        Task<ErrorOr<CreateCarsharingUserRequest>> CreateOrUpdateCarsharingUserAsync(CreateCarsharingUserRequest createCarsharingUserRequest);
        Task<ErrorOr<CarsharingUserResponse>> GetCarsharingUserByUserId(Guid userId);
        Task<ErrorOr<bool>> IsCarsharingUserExists(Guid userId);
    }
}
