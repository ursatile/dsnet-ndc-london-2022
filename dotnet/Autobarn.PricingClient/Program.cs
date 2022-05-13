using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Autobarn.PricingClient {
    internal class Program
    {
        private static Pricer.PricerClient grpcClient;
        private static readonly IConfigurationRoot config = ReadConfiguration();
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress(config.GetConnectionString("AutobarnPricingServerUrl"));
            grpcClient = new Pricer.PricerClient(channel);
            var amqp = config.GetConnectionString("AutobarnRabbitMqConnectionString");
            using var bus = RabbitHutch.CreateBus(amqp);
            var handler = BuildHandler(bus);
            bus.PubSub.Subscribe("Autobarn.PricingClient", handler, x => x.WithAutoDelete());
            Console.WriteLine("Listening for NewVehicleMessages - press Enter to quit");
            Console.ReadLine();
        }

        private static Action<NewVehicleMessage> BuildHandler(IBus bus)
        {
            return async m =>
            {
                Console.WriteLine($"retrieving price for {m.Registration} ({m.Make} {m.Model}, {m.Color}, {m.Year}");
                var request = new PriceRequest
                {
                    Make = m.Make,
                    Color = m.Color,
                    Year = m.Year,
                    Model = m.Model
                };
                var priceReply = await grpcClient.GetPriceAsync(request);
                Console.WriteLine($"Price: {priceReply.Price} {priceReply.CurrencyCode}");
                var newVehiclePriceMessage = m.ToNewVehiclePriceMessage(priceReply.Price, priceReply.CurrencyCode);
                await bus.PubSub.PublishAsync(newVehiclePriceMessage);
            };
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
