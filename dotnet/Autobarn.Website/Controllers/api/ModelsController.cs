using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Autobarn.Website.Models;

namespace Autobarn.Website.Controllers.api {
	[Route("api/[controller]")]
	[ApiController]
	public class ModelsController : ControllerBase {
		private readonly IAutobarnDatabase db;

		public ModelsController(IAutobarnDatabase db) {
			this.db = db;
		}

		[HttpGet]
		public IEnumerable<Model> Get() {
			return db.ListModels();
		}

		[HttpGet("{id}")]
		public IActionResult Get(string id) {
			var vehicleModel = db.FindModel(id);
			if (vehicleModel == default) return NotFound();
            var result = vehicleModel.ToDynamic();
            result._links = new
            {
                self = new
                {
                    href = $"/api/models/{id}"
                }
            };
            result._actions = new
            {
                create = new
                {
                    method = "POST",
                    name = $"Create a new {vehicleModel.Manufacturer.Name} {vehicleModel.Name}",
                    href = $"/api/models/{id}",
                    type = "application/json"
                }
            };
            return Ok(result);
		}

        // POST api/models/{model-code}
        [HttpPost("{id}")]
        public IActionResult Post(string id, [FromBody] VehicleDto dto) {
            var existing = db.FindVehicle(dto.Registration);
            if (existing != default) {
                return Conflict(
                    $"Sorry, there is already a vehicle with registration {dto.Registration} in our database (and listing the same vehicle twice is against the rules!)");
            }
            var vehicleModel = db.FindModel(id);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };
            db.CreateVehicle(vehicle);
            return Created($"/api/vehicles/{vehicle.Registration}", vehicle.ToResource());
        }


	}
}