using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace Autobarn.PricingServer {
	public class Program {
		public static void Main(string[] args) {
			CreateHostBuilder(args).Build().Run();
		}

		private const int HTTP_PORT = 5002;
		private const int HTTPS_PORT = 5003;

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.ConfigureKestrel(options => {
						options.ListenAnyIP(HTTP_PORT, listener => listener.Protocols = HttpProtocols.Http1AndHttp2);
						options.ListenAnyIP(HTTPS_PORT, listener => {
							listener.Protocols = HttpProtocols.Http1AndHttp2;
							listener.UseHttps();
						});
					});
					webBuilder.UseStartup<Startup>();
				});
	}
}
