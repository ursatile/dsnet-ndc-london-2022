using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
			return Ok(vehicleModel);

		}

		public static string ParseModelId(dynamic href) {
			// In a real application, you'd use the API routing tables
			// to extract the parameters from the href, but for now
			// we'll do it based on simple string manipulation.
			var tokens = ((string)href).Split("/");
			return tokens.Last();
		}
	}
}