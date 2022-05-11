using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Autobarn.Website.Controllers.api {
	[Route("api")]
	[ApiController]
	public class ApiController : ControllerBase {
		[HttpGet]
		public IActionResult Get() {
			var result = new {
				message = "Welcome to the Autobarn API",
				_links = new {
					vehicles = new {
						href = "/api/vehicles"
					}
				}
			};
			return new JsonResult(result);
		}
	}
}