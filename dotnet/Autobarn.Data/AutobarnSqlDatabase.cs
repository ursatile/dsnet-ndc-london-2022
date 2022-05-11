using System.Collections.Generic;
using Autobarn.Data.Entities;

namespace Autobarn.Data {
	public class AutobarnSqlDatabase : IAutobarnDatabase {
		private readonly AutobarnDbContext dbContext;

		public AutobarnSqlDatabase(AutobarnDbContext dbContext) => this.dbContext = dbContext;

		public IEnumerable<Vehicle> ListVehicles() => dbContext.Vehicles;

		public IEnumerable<Manufacturer> ListManufacturers() => dbContext.Manufacturers;

		public IEnumerable<Model> ListModels() => dbContext.Models;

		public Vehicle FindVehicle(string registration) => dbContext.Vehicles.Find(registration);

		public Model FindModel(string code) => dbContext.Models.Find(code);

		public Manufacturer FindManufacturer(string code) => dbContext.Manufacturers.Find(code);

		public void AddVehicle(Vehicle vehicle) {
			dbContext.Vehicles.Add(vehicle);
			dbContext.SaveChanges();
		}
	}
}