using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;

namespace Autobarn.PricingServer {
	public class Program {
		public static void Main(string[] args) {
            Console.WriteLine("Starting Autobarn.PricingServer...");
            CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureLogging(logging => {
					logging.ClearProviders();
					logging.AddConsole();
				})
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.ConfigureKestrel(options => {
						var pfxPassword = Environment.GetEnvironmentVariable("UrsatilePfxPassword");
						var https = UseCertIfAvailable(@"d:\workshop.ursatile.com\ursatile.com.pfx", pfxPassword);
						options.ListenAnyIP(5002, listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2);
						options.Listen(IPAddress.Any, 5003, https);
						options.AllowSynchronousIO = true;
					});
					webBuilder.UseStartup<Startup>();
				}
				);

		private static Action<ListenOptions> UseCertIfAvailable(string pfxFilePath, string pfxPassword) {
			if (File.Exists(pfxFilePath)) return listen => listen.UseHttps(pfxFilePath, pfxPassword);
			return listen => listen.UseHttps();
		}
	}
}
