
using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class CarsharingUserController : APIController
    {
        private readonly ICarsharingUsersService _carsharingUserService;
        public CarsharingUserController(ILogger<APIController> logger, ICarsharingUsersService carsharingUsersService) : base(logger)
        {
            _carsharingUserService = carsharingUsersService;
        }

        [AllowAnonymous]
        [HttpPost("CreateOrUpdateCarsharingUser")]
        public async Task<ActionResult> CreateOrUpdateCarsharingUser([FromBody] CreateCarsharingUserRequest request)
        {
            var errorOrCreatedOrUpdated = await _carsharingUserService.CreateOrUpdateCarsharingUserAsync(request);

            if (errorOrCreatedOrUpdated.IsError)
            {
                return BadRequest(errorOrCreatedOrUpdated.Errors);
            }

            return Ok(errorOrCreatedOrUpdated.Value);
        }

        [Authorize(Policy = "user")]
        [HttpGet("GetCarsharingUserByUserId")]
        public async Task<ActionResult> GetCarsharingUserByUserId([FromQuery] string userId)
        {
            var errorOrCarsharingUser = await _carsharingUserService.GetCarsharingUserByUserId(Guid.Parse(userId));

            if (errorOrCarsharingUser.IsError)
            {
                return BadRequest(errorOrCarsharingUser.Errors);
            }

            return Ok(errorOrCarsharingUser.Value);
        }

        [AllowAnonymous]
        [HttpGet("IsCarsharingUserExistsWithUserId")]
        public async Task<ActionResult> IsCarsharingUserExistsWithUserId([FromQuery] string userId)
        {
            var errorOrExistsFlag = await _carsharingUserService.IsCarsharingUserExists(Guid.Parse(userId));

            if (errorOrExistsFlag.IsError)
            {
                return BadRequest(errorOrExistsFlag.Errors);
            }

            return Ok(errorOrExistsFlag.Value);
        }

    }
}
