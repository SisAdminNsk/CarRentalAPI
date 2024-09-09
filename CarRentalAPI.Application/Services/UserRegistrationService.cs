using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Application.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private Context.Context _context;
        public UserRegistrationService(Context.Context context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<ErrorOr<Created>> RegistrateNewUserAsync(UserDTO user)
        {
            try
            {
                var roles = await _context.UsersRoles.ToListAsync();

                var standartUserRole = roles.Where(r => r.Name == "user").ToList();

                var userEntity = new User(user.Id, user.Login, _passwordHasher.Generate(user.Password), standartUserRole);

                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();

                return Result.Created;
            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "UserRegistrationService.Registrate.Failure", 
                    "Error occured while registration new user", metadata);
            }
        }
    }
}
