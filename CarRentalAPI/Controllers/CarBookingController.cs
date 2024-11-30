using CarRentalAPI.Application.Filters;
using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Application.Paginations;
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

        [Authorize(Policy = "user")]
        [HttpGet("GetServerDateTime")]
        public async Task<ActionResult> GetServerDateTime([FromQuery] int hoursOffset)
        {
            var local = DateTime.UtcNow.AddHours(hoursOffset);
            var userLocalTime = DateTime.UtcNow.AddHours(hoursOffset).ToString();

            return Ok(userLocalTime);
        }


        [Authorize(Policy = "user")]
        [HttpPost("CreateOrUpdateCarOrder")]
        public async Task<ActionResult> CreateOrUpdateCarOrder([FromBody] CarOrderRequest request)
        {
            var errorOrOrder = await _carBookingService.CreateOrUpdateCarOrderAsync(request);

            if (errorOrOrder.IsError)
            {
                return BadRequest(errorOrOrder.Errors);
            }

            return Ok(errorOrOrder.Value);
        }

        [AllowAnonymous] // доступно пользователеям только с ролью Admin
        [HttpPost(("OpenCarReservation"))]
        public async Task<ActionResult> OpenCarReservation([FromBody] OpenCarReservationRequest openCarReservationRequest)
        {
            var errorOrOpenedCarReservation = await _carBookingService.OpenCarReservationAsync(openCarReservationRequest);

            if (errorOrOpenedCarReservation.IsError)
            {
                return BadRequest(errorOrOpenedCarReservation.Errors);
            }

            return Ok(errorOrOpenedCarReservation.Value);
        }

        [AllowAnonymous]  // доступно пользователеям только с ролью Admin
        [HttpPost("CloseCarReservation")]
        public async Task<ActionResult> CloseCarReservation([FromBody] CloseCarReservationRequest closeCarReservationRequest)
        {
            var errorOrClosedCarReservation = await _carBookingService.CloseCarReservationAsync(closeCarReservationRequest);

            if (errorOrClosedCarReservation.IsError)
            {
                return BadRequest(errorOrClosedCarReservation.Errors);
            }

            return Ok(errorOrClosedCarReservation.Value);
        }

        [AllowAnonymous]
        [HttpDelete("DeleteCarOrder")]  // доступно пользователеям только с ролью Admin
        public async Task<ActionResult> DeleteCarOrder([FromQuery] Guid orderId)
        {
            var errorOrDeleted = await _carBookingService.DeleteCarOrderAsync(orderId);

            if (errorOrDeleted.IsError)
            {
                return BadRequest(errorOrDeleted.Errors);
            }

            return Ok(errorOrDeleted.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetCarOrdersByUserId")]
        public async Task<ActionResult> GetCarOrdersByUserId([FromQuery] Guid userId)
        {
            var errorOrCarOrders = await _carBookingService.GetCarOrdersByCarsharingUserIdAsync(userId);

            if (errorOrCarOrders.IsError)
            {
                return BadRequest(errorOrCarOrders.Errors);
            }

            return Ok(errorOrCarOrders.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetAllCarOrders")]
        public async Task<ActionResult> GetAllCarOrders()
        {
            var errorOrCarOrders = await _carBookingService.GetAllCarOrdersAsync();

            if (errorOrCarOrders.IsError)
            {
                return BadRequest(errorOrCarOrders.Errors);
            }

            return Ok(errorOrCarOrders.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetAllNotConsideredCarOrders")]
        public async Task<ActionResult> GetAllNotConsideredCarOrders()
        {
            var errorOrNotConsideredCarOrders = await _carBookingService.GetAllNotConsideredCarOrders();

            if (errorOrNotConsideredCarOrders.IsError)
            {
                return BadRequest(errorOrNotConsideredCarOrders.Errors);
            }

            return Ok(errorOrNotConsideredCarOrders.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetAllOpenedCarReservations")]
        public async Task<ActionResult> GetAllOpenedCarReservations()
        {
            var errorOrOpenedCarReservations = await _carBookingService.GetAllOpenedCarOrdersAsync();

            if (errorOrOpenedCarReservations.IsError)
            {
                return BadRequest(errorOrOpenedCarReservations.Errors);
            }

            return Ok(errorOrOpenedCarReservations.Value);
        }

        [Authorize(Policy = "user")]
        [HttpGet("GetOpenedCarReservationsByCarsharingUserId")]
        public async Task<ActionResult> GetOpenedCarReservationsByCarsharingUserId([FromQuery] Guid carsharingUserId)
        {
            var errorOrOpenedCarReservations = await _carBookingService.GetOpenedCarOrdersByCarsharingUserIdAsync(carsharingUserId);

            if (errorOrOpenedCarReservations.IsError)
            {
                return BadRequest(errorOrOpenedCarReservations.Errors);
            }

            return Ok(errorOrOpenedCarReservations.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetAllClosedCarReservations")]
        public async Task<ActionResult> GetAllClosedCarReservations()
        {
            var errorOrClosedCarReservations = await _carBookingService.GetAllClosedCarOrdersAsync();

            if (errorOrClosedCarReservations.IsError)
            {
                return BadRequest(errorOrClosedCarReservations.Errors);
            }

            return Ok(errorOrClosedCarReservations.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetClosedCarReservationsByCarsharingUserId")]
        public async Task<ActionResult> GetClosedCarReservationsByCarsharingUserId([FromQuery] CarOrderFilter filter,
            [FromQuery] SortParams sortParams, [FromQuery] PageParams pageParams, [FromQuery] Guid carsharingUserId)
        {
            var errorOrClosedCarReservations = await _carBookingService.GetClosedCarOrdersByCarsharingUserIdAsync(filter, sortParams, pageParams, carsharingUserId);

            if (errorOrClosedCarReservations.IsError)
            {
                return BadRequest(errorOrClosedCarReservations.Errors);
            }

            return Ok(errorOrClosedCarReservations.Value);
        }

        [AllowAnonymous]
        [HttpGet("IsCarFreeForBooking")]
        public async Task<ActionResult> IsCarFreeForBooking([FromQuery] Guid carId)
        {
            var errorOrCarStatusResponse = await _carBookingService.IsCarFreeForBooking(carId);

            if (errorOrCarStatusResponse.IsError)
            {
                return BadRequest(errorOrCarStatusResponse.Errors);
            }

            return Ok(errorOrCarStatusResponse.Value);
        }
    }
}
