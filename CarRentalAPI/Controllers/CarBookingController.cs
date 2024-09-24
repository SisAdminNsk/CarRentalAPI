using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class CarBookingController : APIController
    {
        private readonly ICarBookingService _carBookingService;
        public CarBookingController(
           ILogger<CarBookingController> logger,
           ICarBookingService carBookingService) : base(logger)
        {
            _carBookingService = carBookingService;
        }

        [AllowAnonymous]
        [HttpPost("CreateCarOrder")]
        public async Task<ActionResult> CreateCarOrder(CreateCarOrderRequest request)
        {
            var errorOrOrder = await _carBookingService.CreateCarOrderAsync(request);

            if (errorOrOrder.IsError)
            {
                return BadRequest(errorOrOrder.Errors);
            }

            return Ok(errorOrOrder.Value);
        }

        [AllowAnonymous] // доступно пользователеям только с ролью Admin
        [HttpPost(("OpenCarReservation"))]
        public async Task<ActionResult> OpenCarReservation(Guid carOrderId, DateTime startOfLease, DateTime endOfLease)
        {
            var errorOrOpenedCarReservation = await _carBookingService.OpenCarReservationAsync(carOrderId, startOfLease, endOfLease);

            if(errorOrOpenedCarReservation.IsError)
            {
                return BadRequest(errorOrOpenedCarReservation.Errors);
            }

            return Ok(errorOrOpenedCarReservation.Value);
        }

        [AllowAnonymous]  // доступно пользователеям только с ролью Admin
        [HttpPost("CloseCarReservation")]
        public async Task<ActionResult> CloseCarReservation(Guid openedCarOrderId, string status)
        {
            var errorOrClosedCarReservation = await _carBookingService.CloseCarReservationAsync(openedCarOrderId, status);

            if(errorOrClosedCarReservation.IsError)
            {
                return BadRequest(errorOrClosedCarReservation.Errors);
            }

            return Ok(errorOrClosedCarReservation.Value);
        }

        [AllowAnonymous]
        [HttpDelete("DeleteCarOrder")]  // доступно пользователеям только с ролью Admin
        public async Task<ActionResult> DeleteCarOrder(Guid orderId)
        {
            var errorOrDeleted = await _carBookingService.DeleteCarOrderAsync(orderId);

            if (errorOrDeleted.IsError)
            {
                return BadRequest(errorOrDeleted.Errors);
            }

            return Ok(errorOrDeleted.Value);
        }
    }
}
