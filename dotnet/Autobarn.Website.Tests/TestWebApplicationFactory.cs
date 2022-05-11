using Autobarn.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Autobarn.Website.Tests {
	public class TestWebApplicationFactory<T> : WebApplicationFactory<T> where T : class {
		protected override void ConfigureWebHost(IWebHostBuilder builder) {
			builder.ConfigureServices(services => {
				services.RemoveAll(typeof(IAutobarnDatabase));
				services.AddSingleton<IAutobarnDatabase, AutobarnCsvFileDatabase>();
			});
		}
	}
}