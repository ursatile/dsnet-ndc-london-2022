using Autobarn.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Autobarn.Data {
	public static class ServiceCollectionExtensions {
		public static IServiceCollection UseAutobarnSqlDatabase(this IServiceCollection services, string sqlConnectionString) {
			var loggerFactory = LoggerFactory.Create(builder => {
				builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information);
				builder.AddConsole();
			});
			services.AddDbContext<AutobarnDbContext>(options => {
				options.UseLazyLoadingProxies();
				options.UseLoggerFactory(loggerFactory);
				options.UseSqlServer(sqlConnectionString);
			});
			services.AddScoped<IAutobarnDatabase, AutobarnSqlDatabase>();
			return services;
		}
	}
}