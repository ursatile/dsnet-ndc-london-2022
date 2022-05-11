using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Autobarn.PricingServer.Services {
	public class VehiclePricingService : Pricer.PricerBase {
		private readonly ILogger<VehiclePricingService> logger;
		public VehiclePricingService(ILogger<VehiclePricingService> logger) {
			this.logger = logger;
		}

		public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
			//TODO: calculate prices properly!
			return Task.FromResult(new PriceReply {
				Price = 5000,
				CurrencyCode = "EUR"
			});
		}
	}
}
