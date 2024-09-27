using CarRentalAPI.Application.Email;
using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Application.Interfaces.Email;
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
        private readonly IEmailConfirmationService _emailConfirmationService;
        private Context.Context _context;
        public UserService(
            Context.Context context,
            IPasswordHasher passwordHasher,
            IJWTProvider jwtProvider,
            IEmailConfirmationService emailConfirmationService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _emailConfirmationService = emailConfirmationService;
        }

        public async Task<ErrorOr<LoginResponse>> LoginAsync(UserLoginRequest loginRequest)
        {
            try
            {
                var user = await _context.Users.
                    Where(user => user.Email == loginRequest.Email).
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
                    return Error.NotFound("UserService.Login.NotFound", description: 
                        $"User with login: {{{loginRequest.Email}}} was not found.");
                }

                var token = _jwtProvider.GenerateToken(user);

                return new LoginResponse(token, user.Id);

            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new() { {"Exception",ex.Message } };

                return Error.Failure("UserService.Login.Failure", "Failure on login", metadata);
            }
        }

        public async Task<ErrorOr<Success>> SendEmailVerificationCodeAsync(string email,
            CancellationToken cancellationToken)
        {
            return await _emailConfirmationService.SendRegistrationCodeMessageAsync(email, cancellationToken);
        }
        public async Task<ErrorOr<Success>> IsUserNotExistsWithData(UserRegistrateRequest data)
        {

            var errors = new List<Error>();

            try
            {
                if (await IsUserAlreadyExistsWithEmail(data.Email))
                {
                    Dictionary<string, object> metadata = new() { { "StatusCode", 409 } };

                    errors.Add(Error.Conflict("UserService.Registrate.Conflict.Email",
                        $"User with login: {data.Email} already exists.", metadata));
                }

                if (await IsUserAlreadyExistsWithName(data.Username))
                {
                    Dictionary<string, object> metadata = new() { { "StatusCode", 409 } };

                    errors.Add(Error.Conflict("UserService.Registrate.Conflict.Username",
                        $"User with name: {data.Username} already exists.", metadata));
                }

                if (errors.Count != 0)
                {
                    return errors;
                }

                return Result.Success;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> metadata = new() { { "Exception", ex.Message } };

                return Error.Failure(code: "UserService.Registrate.Failure",
                    "Error occured while registration new user", metadata);
            }
        }
        private async Task<bool> IsUserAlreadyExistsWithEmail(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if(user is not null)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> IsUserAlreadyExistsWithName(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == userName);

            if (user is not null)
            {
                return true;
            }

            return false;

        }
        private async Task<User> CreateUserAsync(UserRegistrateRequest user)
        {
            var roles = await _context.UsersRoles.ToListAsync();

            var standartUserRole = roles.Where(r => r.Name == "user").ToList();

            var userEntity = new User(
                Guid.NewGuid(),
                user.Email,
                _passwordHasher.Generate(user.Password),
                user.Username,
                standartUserRole);

            return userEntity;
        }

        public async Task<ErrorOr<Created>> RegistrateIfVerifiedAsync(
            UserRegistrateRequest userRegistrationRequest,
            string code,
            CancellationToken cancellationToken)
        {
            var result = _emailConfirmationService.VerifyCode(userRegistrationRequest.Email, code);

            if (result.IsError == true)
            {
                return Error.Failure(description: "Verification code was failed.");
            }

            var verificationResult = result.Value;

            switch (verificationResult)
            {
                case VerificationResult.Verifed:
                    return await OnVerifedAction(userRegistrationRequest, cancellationToken);

                case VerificationResult.Outdated:
                    return OnOutdatedAction();

                case VerificationResult.Wrong:
                    return OnWrongAction();

                default:
                    return Error.Failure("UserService.OnCheckigVerificationResult.Failure",
                        "Произошла неизвестная ошибка.");
            }
        }
        private async Task<ErrorOr<Created>> OnVerifedAction(

            UserRegistrateRequest userRegistrationRequest,
            CancellationToken cancellationToken)
        {
            var user = await CreateUserAsync(userRegistrationRequest);

            try
            {
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Dictionary<string, object> metadata = new() { { "StatusCode", 500 }, {"Exception",ex.Message } };

                return Error.Failure("UserService.OnVerifiedAction.Failure", "Somethig went wrong on code verification",
                    metadata);
            }

            return Result.Created;
        }

        private Error OnOutdatedAction()
        {
            return Error.Forbidden(code: "Outdated", description: "Code was outdated.");
        }

        private Error OnWrongAction()
        {
            return Error.Forbidden(code: "WrongCode", description: "Wrong code.");
        }
    }
}
