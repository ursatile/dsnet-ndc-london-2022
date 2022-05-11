using Autobarn.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Int32;

namespace Autobarn.Data {
	public class AutobarnCsvFileDatabase : IAutobarnDatabase {
		private static readonly IEqualityComparer<string> collation = StringComparer.OrdinalIgnoreCase;

		private readonly Dictionary<string, Manufacturer> manufacturers = new Dictionary<string, Manufacturer>(collation);
		private readonly Dictionary<string, Model> models = new Dictionary<string, Model>(collation);
		private readonly Dictionary<string, Vehicle> vehicles = new Dictionary<string, Vehicle>(collation);
		private readonly ILogger<AutobarnCsvFileDatabase> logger;

		public AutobarnCsvFileDatabase(ILogger<AutobarnCsvFileDatabase> logger) {
			this.logger = logger;
			ReadManufacturersFromCsvFile("manufacturers.csv");
			ReadModelsFromCsvFile("models.csv");
			ReadVehiclesFromCsvFile("vehicles.csv");
			ResolveReferences();
		}

		private void ResolveReferences() {
			foreach (var mfr in manufacturers.Values) {
				mfr.Models = models.Values.Where(m => m.ManufacturerCode == mfr.Code).ToList();
				foreach (var model in mfr.Models) model.Manufacturer = mfr;
			}

			foreach (var model in models.Values) {
				model.Vehicles = vehicles.Values.Where(v => v.ModelCode == model.Code).ToList();
				foreach (var vehicle in model.Vehicles) vehicle.VehicleModel = model;
			}
		}

		private string ResolveCsvFilePath(string filename) {
			var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var csvFilePath = Path.Combine(directory, "csv-data");
			return Path.Combine(csvFilePath, filename);
		}

		private void ReadVehiclesFromCsvFile(string filename) {
			var filePath = ResolveCsvFilePath(filename);
			foreach (var line in File.ReadAllLines(filePath)) {
				var tokens = line.Split(",");
				var vehicle = new Vehicle {
					Registration = tokens[0],
					ModelCode = tokens[1],
					Color = tokens[2]
				};
				if (TryParse(tokens[3], out var year)) vehicle.Year = year;
				vehicles[vehicle.Registration] = vehicle;
			}
			logger.LogInformation($"Loaded {vehicles.Count} models from {filePath}");
		}

		private void ReadModelsFromCsvFile(string filename) {
			var filePath = ResolveCsvFilePath(filename);
			foreach (var line in File.ReadAllLines(filePath)) {
				var tokens = line.Split(",");
				var model = new Model {
					Code = tokens[0],
					ManufacturerCode = tokens[1],
					Name = tokens[2]
				};
				models.Add(model.Code, model);
			}
			logger.LogInformation($"Loaded {models.Count} models from {filePath}");
		}

		private void ReadManufacturersFromCsvFile(string filename) {
			var filePath = ResolveCsvFilePath(filename);
			foreach (var line in File.ReadAllLines(filePath)) {
				var tokens = line.Split(",");
				var mfr = new Manufacturer {
					Code = tokens[0],
					Name = tokens[1]
				};
				manufacturers.Add(mfr.Code, mfr);
			}
			logger.LogInformation($"Loaded {manufacturers.Count} manufacturers from {filePath}");
		}

		public int CountVehicles() => vehicles.Count;

		public IEnumerable<Vehicle> ListVehicles() => vehicles.Values;

		public IEnumerable<Manufacturer> ListManufacturers() => manufacturers.Values;

		public IEnumerable<Model> ListModels() => models.Values;

		public Vehicle FindVehicle(string registration) => vehicles.GetValueOrDefault(registration);

		public Model FindModel(string code) => models.GetValueOrDefault(code);

		public Manufacturer FindManufacturer(string code) => manufacturers.GetValueOrDefault(code);

		public void CreateVehicle(Vehicle vehicle) {
			vehicle.VehicleModel.Vehicles.Add(vehicle);
			UpdateVehicle(vehicle);
		}

		public void UpdateVehicle(Vehicle vehicle) {
			vehicles[vehicle.Registration] = vehicle;
		}

		public void DeleteVehicle(Vehicle vehicle) {
			var model = FindModel(vehicle.ModelCode);
			model.Vehicles.Remove(vehicle);
			vehicles.Remove(vehicle.Registration);
		}
	}
}