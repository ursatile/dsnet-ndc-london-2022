using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autobarn.Data;
using Autobarn.Data.Entities;

namespace Autobarn.Website.Controllers {
	public class ModelsController : Controller {
		private readonly IAutobarnDatabase db;

		public ModelsController(IAutobarnDatabase db) {
			this.db = db;
		}

		public IActionResult Vehicles(string id) {
			var model = db.ListModels().FirstOrDefault(m => m.Code == id);
			return View(model);
		}

		public IActionResult Index() {
			var models = db.ListModels();
			return View(models);
		}
	}
}
