using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Messaging;
using Autobarn.Website.Models;
using Azure.Messaging.ServiceBus;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
	[Route("api")]
	[ApiController]
	public class ApiController : ControllerBase {
		[HttpGet]
		public IActionResult Get() {
			var result = new {
				_links = new {
					vehicles = new {
						href = "/api/vehicles"
					}
				},
				_actions = new {
					create = new {
						name = "Add a car",
						href = "/api/vehicles/",
						method = "POST",
						type = "application/json"
					}
				}
			};
			return Ok(result);
		}
	}

	[Route("api/[controller]")]
	[ApiController]
	public class VehiclesController : ControllerBase {
		private readonly IAutobarnDatabase db;
		private readonly IAutobarnServiceBus bus;

		// GET: api/vehicles
		public VehiclesController(IAutobarnDatabase db, IAutobarnServiceBus bus) {
			this.db = db;
			this.bus = bus;
		}
		[HttpGet]
		public IEnumerable<Vehicle> Get() {
			return db.ListVehicles();
		}

		// GET api/vehicles/5
		[HttpGet("{id}")]
		public Vehicle Get(string id) {
			return db.FindVehicle(id);
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
			db.AddVehicle(vehicle);
			bus.PublishNewVehicleNotification(dto);
			return Ok(dto);
		}

		// PUT api/vehicles/5
		[HttpPut("{id}")]
		public IActionResult Put(string id, [FromBody] VehicleDto dto) {
			return Ok(dto);
		}

		// DELETE api/vehicles/5
		[HttpDelete("{id}")]
		public IActionResult Delete(string id) {
			return Ok(id);
		}
	}
}
