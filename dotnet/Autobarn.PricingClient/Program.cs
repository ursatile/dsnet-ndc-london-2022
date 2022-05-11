using Autobarn.PricingServer;
using Grpc.Net.Client;
using System;

namespace Autobarn.PricingClient {
	class Program {
		static void Main(string[] args) {
			using var channel = GrpcChannel.ForAddress("https://localhost:5003");
			var grpcClient = new Pricer.PricerClient(channel);
			Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit):");
			while (true) {
				Console.ReadKey(true);
				var request = new PriceRequest {
					ModelCode = "volkwsagen-beetle",
					Color = "Green",
					Year = 1985
				};
				var reply = grpcClient.GetPrice(request);
				Console.WriteLine($"Price: {reply.Price}");
			}
		}
	}
}
