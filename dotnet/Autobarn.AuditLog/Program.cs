using System;
using System.IO;
using System.Threading;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Autobarn.AuditLog {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        static void Main(string[] args) {
            var amqp = config.GetConnectionString("AutobarnRabbitMqConnectionString");
            using var bus = RabbitHutch.CreateBus(amqp);
            bus.PubSub.Subscribe<NewVehicleMessage>("Autobarn.AuditLog", 
                HandleNewVehicleMessage,
                x => x.WithAutoDelete());
            Console.WriteLine("Listening for NewVehicleMessages - press Enter to quit");
            Console.ReadLine();
        }

        private static void HandleNewVehicleMessage(NewVehicleMessage m) {
            Console.WriteLine($@"{m.Make} {m.Model} ({m.Color}, {m.Year})");
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
