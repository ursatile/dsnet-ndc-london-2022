using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Autobarn.Messages;
using EasyNetQ;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
	[Route("api/[controller]")]
	[ApiController]
	public class VehiclesController : ControllerBase {
		private readonly IAutobarnDatabase db;
		private readonly IBus bus;

		public VehiclesController(IAutobarnDatabase db, IBus bus) {
			this.db = db;
			this.bus = bus;
		}

		// GET: api/vehicles
		[HttpGet]
		public IEnumerable<Vehicle> Get() {
			return db.ListVehicles();
		}

		// GET api/vehicles/ABC123
		[HttpGet("{id}")]
		public IActionResult Get(string id) {
			var vehicle = db.FindVehicle(id);
			if (vehicle == default) return NotFound();
			return Ok(vehicle);
		}

		// POST api/vehicles
		[HttpPost]
		public IActionResult Post([FromBody] VehicleDto dto) {
			var vehicleModel = db.FindModel(dto.ModelCode);
			var vehicle = new Vehicle {
				Registration = dto.Registration,
				Color = dto.Color,
				Year = dto.Year,
				VehicleModel = vehicleModel
			};
			db.CreateVehicle(vehicle);
			PublishNewVehicleMessage(vehicle);
			return Ok(dto);
		}

		private void PublishNewVehicleMessage(Vehicle vehicle) {
			var message = new NewVehicleMessage() {
				Registration = vehicle.Registration,
				Manufacturer = vehicle.VehicleModel?.Manufacturer?.Name,
				ModelName = vehicle.VehicleModel?.Name,
				ModelCode = vehicle.VehicleModel?.Code,
				Color = vehicle.Color,
				Year = vehicle.Year,
				ListedAtUtc = DateTime.UtcNow
			};
			bus.PubSub.Publish(message);
		}

		// PUT api/vehicles/ABC123
		[HttpPut("{id}")]
		public IActionResult Put(string id, [FromBody] VehicleDto dto) {
			var vehicleModel = db.FindModel(dto.ModelCode);
			var vehicle = new Vehicle {
				Registration = dto.Registration,
				Color = dto.Color,
				Year = dto.Year,
				ModelCode = vehicleModel.Code
			};
			db.UpdateVehicle(vehicle);
			return Ok(dto);
		}

		// DELETE api/vehicles/ABC123
		[HttpDelete("{id}")]
		public IActionResult Delete(string id) {
			var vehicle = db.FindVehicle(id);
			if (vehicle == default) return NotFound();
			db.DeleteVehicle(vehicle);
			return NoContent();
		}
	}
}
