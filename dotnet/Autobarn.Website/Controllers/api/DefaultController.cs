using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Autobarn.Website.Controllers.api {
    [Route("api")]
    [ApiController]
    public class DefaultController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            var result = new {
                message = "Welcome to the Autobarn API",
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                _links = new {
                    vehicles = new {
                        href = "/api/vehicles"
                    },
                    models = new {
                        href = "/api/models"
                    }
                }
            };
            return Ok(result);
        }
    }
}