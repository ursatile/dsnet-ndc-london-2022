using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;

namespace Autobarn.Website {
	public class Program {
		public static void Main(string[] args) {
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => {
					webBuilder.ConfigureKestrel(options => {
						var pfxPassword = Environment.GetEnvironmentVariable("UrsatilePfxPassword");
						var https = UseCertIfAvailable(@"d:\workshop.ursatile.com\ursatile.com.pfx", pfxPassword);
						options.ListenAnyIP(5000, listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2);
						options.Listen(IPAddress.Any, 5001, https);
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