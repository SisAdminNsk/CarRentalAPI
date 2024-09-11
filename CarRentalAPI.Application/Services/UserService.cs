using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using CarRentalAPI.Core;

using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJWTProvider _jwtProvider;
        private Context.Context _context;
        public UserService(Context.Context context, IPasswordHasher passwordHasher, IJWTProvider jwtProvider)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<ErrorOr<List<Role>>> GetRolesByLoginAsync(string login)
        {
            try
            {
                var user = await _context.Users.
                    Where(user => user.Login == login).
                    Include(user => user.Roles).
                    AsNoTracking().
                    FirstOrDefaultAsync();

                if(user is not null)
                {
                    return user.Roles;
                }

                return Error.NotFound(code: "UserService.GetRolesByLogin.NotFound",
                    description: $"user with login: {login} was not found.");

            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "UserService.GetRolesByLogin.Failure",
                    "Error occured while getting user by login", metadata);
            }
        }

        public async Task<ErrorOr<string>> LoginAsync(UserLoginRequest loginRequest)
        {
            try
            {
                var user = await _context.Users.
                    Where(user => user.Login == loginRequest.Login).
                    Include(user => user.Roles).FirstOrDefaultAsync();

                if (user is not null)
                {
                    var result = _passwordHasher.VerifyPassword(loginRequest.Password, user.Password);

                    if(result is not true)
                    {
                        return Error.Conflict("UserService.Login.Conflict", description: "Wrong password.");
                    }
                }
                else
                {
                    return Error.NotFound("UserService.Login.NotFound", description: $"User with login: {{{loginRequest.Login}}} was not found.");
                }

                var token = _jwtProvider.GenerateToken(user);

                return token;

            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);


                return Error.Failure("UserService.Login.Failure", "Failure on login", metadata);
            }
        }

        public async Task<ErrorOr<Created>> RegistrateNewUserAsync(UserDTO user)
        {
            try
            {
                if (await isUserAlreadyExists(user.Login))
                {
                    Dictionary<string, object> metadata = new();
                    metadata.Add("StatusCode", 409);


                    return Error.Conflict("UserService.Registrate.Conflict", $"User with login: {user.Login} already exists.", metadata);
                }


                var roles = await _context.UsersRoles.ToListAsync();

                var standartUserRole = roles.Where(r => r.Name == "user").ToList();

                var userEntity = new User(Guid.NewGuid(), user.Login, _passwordHasher.Generate(user.Password), standartUserRole);

                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();

                return Result.Created;
            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new();
                metadata.Add("Exception", ex.Message);

                return Error.Failure(code: "UserService.Registrate.Failure", 
                    "Error occured while registration new user", metadata);
            }
        }

        private async Task<bool> isUserAlreadyExists(string userLogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin);

            if(user is not null)
            {
                return true;
            }

            return false;
        }
    }
}
