using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Autobarn.Notifier {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static HubConnection hub;
        static async Task Main(string[] args) {
            var hubUrl = config.GetConnectionString("AutobarnSignalRHubUrl");
            hub = new HubConnectionBuilder().WithUrl(hubUrl).Build();
            await hub.StartAsync();
            Console.WriteLine("Connected to SignalR Hub!");
            var amqp = config.GetConnectionString("AutobarnRabbitMqConnectionString");
            using var bus = RabbitHutch.CreateBus(amqp);

            bus.PubSub.Subscribe<NewVehiclePriceMessage>("Autobarn.Notifier",
                HandleNewVehiclePriceMessage,
                x => x.WithAutoDelete());
            Console.WriteLine("Listening for NewVehicleMessages - press Enter to quit");
            Console.ReadLine();
        }

        private static async void HandleNewVehiclePriceMessage(NewVehiclePriceMessage m) {
            var json = JsonConvert.SerializeObject(m);
            await hub.SendAsync("NotifyPeopleWhoAreLookingAtMyWebsite", "autobarn.notifier", json);
            Console.WriteLine($"{m.Registration}: {m.Make} {m.Model} ({m.Color}, {m.Year})");
            Console.WriteLine($"   Calculated price: {m.Price} {m.CurrencyCode}");
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
