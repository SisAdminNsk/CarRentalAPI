using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : APIController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService, ILogger<UserController> logger) : base(logger)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("RegistrateNewUser")]
        public async Task<ActionResult> RegistrateNewUser([FromBody] UserDTO user)
        {
            var errorOrCreated = await _userService.RegistrateNewUserAsync(user);

            if (errorOrCreated.IsError)
            {
                return BadRequest(errorOrCreated.Errors);
            }

            return Ok(errorOrCreated.Value);
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
