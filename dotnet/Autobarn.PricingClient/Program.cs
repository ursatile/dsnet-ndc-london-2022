using Autobarn.Messages;
using Autobarn.PricingServer;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Autobarn.PricingClient {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private const string SUBSCRIBER_ID = "Autobarn.PricingClient";
        private static Pricer.PricerClient grpcClient;
        private static IBus bus;

        static async Task Main(string[] args) {
            Console.WriteLine("Starting Autobarn.PricingClient...");
            var channel = GrpcChannel.ForAddress(config["AutobarnPricingServerUrl"]);
            grpcClient = new Pricer.PricerClient(channel);

            bus = RabbitHutch.CreateBus(config.GetConnectionString("AutobarnRabbitMQ"));
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(SUBSCRIBER_ID, HandleNewVehicleMessage);
            Console.WriteLine("Connected! Listening for NewVehicleMessage messages.");
            Console.ReadKey(true);
        }

        private static async Task HandleNewVehicleMessage(NewVehicleMessage incomingMessage) {
            var request = new PriceRequest {
                ModelCode = incomingMessage.ModelCode,
                Color = incomingMessage.Color,
                Year = incomingMessage.Year
            };
            var reply = await grpcClient.GetPriceAsync(request);
            var outgoingMessage = incomingMessage.ToNewVehiclePriceMessage(reply.Price, reply.CurrencyCode);
            await bus.PubSub.PublishAsync(outgoingMessage);
        }

        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
