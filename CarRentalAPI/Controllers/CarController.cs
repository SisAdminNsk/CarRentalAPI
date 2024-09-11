using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : APIController
    {
        private readonly ICarService _carService;
        private readonly IGarageManagmentService _garageManagmentService;

        public CarController(
            ILogger<CarController> logger,
            ICarService carService,
            IGarageManagmentService garageManagmentService) : base(logger)
        {
            _carService = carService;
            _garageManagmentService = garageManagmentService;
        }
        [Authorize(Policy = "user")]

        [HttpGet("GetAllCars")]
        public async Task<ActionResult> GetAllCars()
        {
            var errorOrAllCars = await _carService.ShowAllCarsAsync();

            if (errorOrAllCars.IsError)
            {
                return BadRequest(errorOrAllCars.Errors);
            }

            return Ok(errorOrAllCars.Value);
        }
        [Authorize(Policy = "admin")]

        [HttpPut("AddNewCar")]
        public async Task<ActionResult> AddNewCar(CarDTO car)
        {
            var errorOrCreated = await _garageManagmentService.AddNewCarAsync(car);

            if (errorOrCreated.IsError)
            {
                return BadRequest(errorOrCreated.Errors);
            }

            return Ok(errorOrCreated.Value);
        }
    }
}