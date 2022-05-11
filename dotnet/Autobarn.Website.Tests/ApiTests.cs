using Autobarn.Data.Entities;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Autobarn.Website.Tests {
	public class ApiTests : IClassFixture<TestWebApplicationFactory<Startup>> {
		private readonly HttpClient client;

		public ApiTests(TestWebApplicationFactory<Startup> factory) {
			client = factory.CreateClient();
		}

		[Fact]
		public async void GET_vehicles_returns_success_status_code() {
			var response = await client.GetAsync("/api/vehicles");
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async void GET_vehicles_returns_vehicle_data() {
			var result = await client.GetVehicles();
			var items = result.items;
			((int)items.Count).ShouldBeGreaterThan(0);
		}

		[Fact]
		public async void GET_vehicles_includes_hypermedia_links() {
			var result = await client.GetVehicles();
			var links = result._links;
			((string)links.next.href).ShouldStartWith("/api/vehicles");
		}

		[Fact]
		public async void POST_creates_vehicle() {
			var registration = Guid.NewGuid().ToString("N");
			var vehicle = new {
				modelCode = "volkswagen-beetle",
				registration,
				color = "Green",
				year = "1985"
			};
			var content = new StringContent(JsonConvert.SerializeObject(vehicle), Encoding.UTF8, "application/json");
			var response = await client.PostAsync($"/api/vehicles", content);
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
			var (_, result) = await client.GetVehicle(registration);
			result.Color.ShouldBe("Green");
			await client.DeleteAsync($"/api/vehicles/{registration}");
		}


		[Fact]
		public async void PUT_creates_vehicle() {
			var registration = Guid.NewGuid().ToString("N");
			await client.PutVolkswagen(registration, "Green", 1985);
			var (_, vehicle) = await client.GetVehicle(registration);
			vehicle.Color.ShouldBe("Green");
			await client.DeleteAsync($"/api/vehicles/{registration}");
		}

		[Fact]
		public async void PUT_updates_vehicle() {
			var registration = Guid.NewGuid().ToString("N");
			await client.PutVolkswagen(registration, "Green", 1985);
			var (_, vehicle) = await client.GetVehicle(registration);
			vehicle.Color.ShouldBe("Green");
			await client.PutVolkswagen(registration, "Brown", 1987);
			(_, vehicle) = await client.GetVehicle(registration);
			vehicle.Color.ShouldBe("Brown");
			await client.DeleteAsync($"/api/vehicles/{registration}");
		}

		[Fact]
		public async void Vehicles_Pagination_Tests() {
			var page1 = await client.GetVehicles();
			((string)page1._links.previous).ShouldBe(null);
			var page2 = await client.Get((string)page1._links.next.href);
			((string)page2._links.previous.href).ShouldNotBe(null);
		}
	}

	public static class HttpClientExtensions {
		public static async Task<dynamic> Get(this HttpClient client, string url) {
			var response = await client.GetAsync((string)url);
			var json = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<dynamic>(json);
		}

		public static async Task<dynamic> GetVehicles(this HttpClient client) {
			var response = await client.GetAsync("/api/vehicles");
			var json = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<dynamic>(json);
		}

		public static async Task<HttpResponseMessage> PutVolkswagen(this HttpClient client, string registration, string color, int year) {
			var vehicle = new {
				_links = new { vehicleModel = new { href = "/api/models/volkswagen-beetle" } },
				registration, color, year
			};
			var content = new StringContent(JsonConvert.SerializeObject(vehicle), Encoding.UTF8, "application/json");
			return await client.PutAsync($"/api/vehicles/{registration}", content);
		}

		public static async Task<(HttpResponseMessage, Vehicle)> GetVehicle(this HttpClient client, string registration) {
			var response = await client.GetAsync($"/api/vehicles/{registration}");
			var json = response.Content.ReadAsStringAsync().Result;
			var vehicle = JsonConvert.DeserializeObject<Vehicle>(json);
			return (response, vehicle);
		}

		public static async Task<HttpResponseMessage> DeleteVehicle(this HttpClient client, string registration) {
			return await client.DeleteAsync($"/api/vehicles/{registration}");
		}
	}
}