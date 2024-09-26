using AutoMapper;
using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Application.Services
{
    public class CarsharingUsersService : ICarsharingUsersService
    {
        private readonly IMapper _mapper;
        private readonly Context.Context _context;

        public CarsharingUsersService(Context.Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ErrorOr<CreateCarsharingUserRequest>> CreateOrUpdateCarsharingUserAsync(CreateCarsharingUserRequest createCarsharingUserRequest)
        {
            try
            {
                var foreignUserEntity = await _context.Users.FindAsync(createCarsharingUserRequest.UserId);

                if(foreignUserEntity is null) 
                {
                    return Error.NotFound("CarsharingUsersService.CreateOrUpdateCarsharingUserAsync.NotFound", 
                        $"User with GUID: {createCarsharingUserRequest.UserId} was not found in database.");
                }

                var carsharingUser = await _context.CarsharingUsers.
                    Where(cu => cu.UserId == createCarsharingUserRequest.UserId).
                    FirstOrDefaultAsync();

                if(carsharingUser is null)
                {
                    var newCarsharingUser = new CarsharingUser
                    (
                        foreignUserEntity,
                        createCarsharingUserRequest.Age,
                        createCarsharingUserRequest.Name,
                        createCarsharingUserRequest.Surname,
                        createCarsharingUserRequest.Patronymic,
                        createCarsharingUserRequest.Phone
                    );

                    await _context.AddAsync(newCarsharingUser);
                }
                else
                {
                    carsharingUser.Age = createCarsharingUserRequest.Age;
                    carsharingUser.Name = createCarsharingUserRequest.Name;
                    carsharingUser.Surname = createCarsharingUserRequest.Surname;
                    carsharingUser.Patronymic = createCarsharingUserRequest.Patronymic;
                    carsharingUser.Phone = createCarsharingUserRequest.Phone;
                }

                await _context.SaveChangesAsync();

                return createCarsharingUserRequest;
            }
            catch(Exception ex)
            {
                return Error.Failure("CarsharingUsersService.IsCarsharingUserExists.Failure", ex.Message);
            }
        }

        public async Task<ErrorOr<CarsharingUserResponse>> GetCarsharingUserByUserId(Guid userId)
        {
            try
            {
                var carsharingUser = await _context.CarsharingUsers.
                    AsNoTracking().
                    Where(cu => cu.UserId == userId).
                    FirstOrDefaultAsync();

                if(carsharingUser is null)
                {
                    return Error.NotFound("CarsharingUsersService.GetCarsharingUserByUserId.NotFound",
                        $"User with GUID: {userId} was not found in database.");
                }

                return _mapper.Map<CarsharingUserResponse>(carsharingUser);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarsharingUsersService.GetCarsharingUserByUserId.Failure", ex.Message);
            }
        }

        public async Task<ErrorOr<bool>> IsCarsharingUserExists(Guid userId)
        {
            try
            {
                return await _context.CarsharingUsers.
                    AsNoTracking().
                    AnyAsync(cu => cu.UserId == userId);
            }
            catch(Exception ex)
            {
                return Error.Failure("CarsharingUsersService.IsCarsharingUserExists.Failure", ex.Message);
            }
        }
    }
}
