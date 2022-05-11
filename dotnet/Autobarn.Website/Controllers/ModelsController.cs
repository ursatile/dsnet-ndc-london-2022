using Autobarn.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
