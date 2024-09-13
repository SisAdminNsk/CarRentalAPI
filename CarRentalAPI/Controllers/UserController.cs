using CarRentalAPI.Application.Email;
using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CarRentalAPI.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UserController : APIController
    {
        private readonly string _sessionId = "SessionId";

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
            HttpContext.Session.SetString(_sessionId, HttpContext.Session.Id);        

            var errorOrVerificationCode = await _userService.SendEmailVerificationCodeAsync(request.Email, cancellationToken);

            if (errorOrVerificationCode.IsError)
            {
                return BadRequest(errorOrVerificationCode.Errors);
            }

            SaveVerificationCodeDetailsToSession(request.Email, errorOrVerificationCode.Value);

            return Ok(new EmailMessageResponse(200, $"Код подтверждения успешно отпрвлен по адресу: {request.Email}."));
        }

        private void SaveVerificationCodeDetailsToSession(string userEmail, VerificationCodeDetails code)
        {
            var serializedVerificaitonCodeDetails = JsonSerializer.Serialize(code);

            HttpContext.Session.SetString(userEmail, serializedVerificaitonCodeDetails);
        }

        private ErrorOr<VerificationCodeDetails> TryGetVerficicationCodeDetailsFromSession(string userEmail)
        {
            var serializedVerificationCodeDetails = HttpContext.Session.GetString(userEmail);

            if(serializedVerificationCodeDetails is not null)
            {
                return JsonSerializer.Deserialize<VerificationCodeDetails>(serializedVerificationCodeDetails);
            }

            return Error.NotFound("UserController.VerificationCodeDetails.NotFound",
                "Код подтверждения неактуален. Попробуйте получить новый.");
        }

        [AllowAnonymous]
        [HttpPost("EndRegistrateUser")]
        public async Task<ActionResult> EndRegistrateUser([FromBody] UserRegistrateRequest request, string code,
            CancellationToken cancellationToken)
        {
            if (HttpContext.Session.Id == HttpContext.Session.GetString(_sessionId))
            {
                var errorOrVerificationCode = TryGetVerficicationCodeDetailsFromSession(request.Email);

                if (!errorOrVerificationCode.IsError)
                {
                    var serverCode = errorOrVerificationCode.Value;

                    var errorOrCreated = await _userService.RegistrateIfVerifiedAsync(
                        serverCode,
                        request,
                        code, 
                        cancellationToken);

                    if (errorOrCreated.IsError)
                    {
                        return BadRequest(errorOrCreated.Errors);
                    }

                    return Ok(request);
                }   
                
                return BadRequest(errorOrVerificationCode.Errors);
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
