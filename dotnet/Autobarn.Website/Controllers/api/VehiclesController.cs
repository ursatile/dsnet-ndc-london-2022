using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase {
        private readonly IAutobarnDatabase db;

        public VehiclesController(IAutobarnDatabase db) {
            this.db = db;
        }

        // GET: api/vehicles
        [HttpGet]
        public IActionResult Get(char index) {
            
            var total = db.GetDistinctFirstLetters();
            if (char.IsWhiteSpace(index) || !char.IsLetter(index) )
            {
                index = total.First();
            }
            index = char.ToUpper(index);
            var items = db.ListVehicles().Where(x => x.Registration.StartsWith(index.ToString(), StringComparison.CurrentCultureIgnoreCase));
            
            var result = new {
                _links = Hal.Paginate("/api/vehicles", index,  total.ToList()),
                items
            };
            return Ok(result);
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
            return Ok(dto);
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
