using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UserController : APIController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService, ILogger<UserController> logger) : base(logger)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("BeginRegistrateUser")]
        public async Task<ActionResult> BeginRegistrateUser([FromBody] UserRegistrateRequest request,
            CancellationToken cancellationToken)
        {
            HttpContext.Session.SetString("SessionId", HttpContext.Session.Id);

            var errorOrVerificationCode = await _userService.SendEmailVerificationCodeAsync(request.Email, cancellationToken);

            if (errorOrVerificationCode.IsError)
            {
                return BadRequest(errorOrVerificationCode.Errors);
            }

            return Ok(new EmailMessageResponse(200, $"Код подтверждения регистрации отправлен по адресу: {request.Email}"));
        }

        [AllowAnonymous]
        [HttpPost("EndRegistrateUser")]
        public async Task<ActionResult> EndRegistrateUser([FromBody] UserRegistrateRequest request, string code,
            CancellationToken cancellationToken)
        {
            if (HttpContext.Session.Id == HttpContext.Session.GetString("SessionId"))
            {
                var errorOrCreated = await _userService.RegistrateIfVerifiedAsync(request, code, cancellationToken);

                if(errorOrCreated.IsError)
                {
                    return BadRequest(errorOrCreated.Errors);
                }

                return Ok(request);
            }

            return Forbid();            
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserLoginRequest loginRequest)
        {
            var errorOrToken = await _userService.LoginAsync(loginRequest);

            if(errorOrToken.IsError)
            {
                return BadRequest(errorOrToken.Errors);
            }

            return Ok(errorOrToken.Value);
        }
    }
}
