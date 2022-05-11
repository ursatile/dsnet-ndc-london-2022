using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Messaging;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.Website {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {

			var loggerFactory = LoggerFactory.Create(builder => {
				builder
					.AddConsole(_ => { })
					.AddFilter((category, level) =>
						category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information);
			});

			services.AddRouting(options => options.LowercaseUrls = true);
			services.AddControllersWithViews().AddNewtonsoftJson();

#if DEBUG
			services.AddRazorPages().AddRazorRuntimeCompilation();
#else
			services.AddRazorPages();
#endif
			var sqlConnectionString = Configuration.GetConnectionString("AutobarnSqlConnectionString");
			services.AddDbContext<AutobarnDbContext>(options => {
				options.UseLazyLoadingProxies();
				options.UseLoggerFactory(loggerFactory);
				options.UseSqlServer(sqlConnectionString);
			});
			services.AddScoped<IAutobarnDatabase, AutobarnSqlDatabase>();
			var busConnectionString = Configuration.GetConnectionString("AzureServiceBusConnectionString");
			services.AddSingleton(_ => new ServiceBusClient(busConnectionString));

			services.AddSingleton<IAutobarnServiceBus>(s => new AutobarnAzureServiceBus(
				s.GetRequiredService<ServiceBusClient>(),
				Configuration["ServiceBus:TopicName"]));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
