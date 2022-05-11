using System;
using EasyNetQ;
using System.Threading.Tasks;
using System.Diagnostics;

// dotnet new console -o Subscriber
// dotnet add package EasyNetQ
// paste this code into Program.cs
// dotnet run
namespace Subscriber {
	class Program {
		static async Task Main(string[] args) {
            var subscriberId = $"ARCHDAYS_WORKSHOP_DEMO_SUBSCRIBER_{Environment.MachineName}_{Process.GetCurrentProcess().Id}";
            Console.WriteLine($"Subscriber ID: {subscriberId}");
			var bus = RabbitHutch.CreateBus("amqps://uzvpuvak:xnIzcflkcIHZmkgqe4uA-Jp9rvKgi1pX@rattlesnake.rmq.cloudamqp.com/uzvpuvak");
            bus.PubSub.Subscribe<ArchdaysMessage>(subscriberId, msg => {
                Console.WriteLine(msg.MessageBody);
                Console.WriteLine($"     - received at {msg.CreatedAt}");
                Console.WriteLine(String.Empty.PadRight(72, '-'));
            });
            Console.WriteLine("Listening for messages. Press Enter to exit");
            Console.ReadLine();
		}
	}
	public class ArchdaysMessage {
		public DateTimeOffset CreatedAt { get; set; }
		public string MessageBody { get; set; }
	}
}
