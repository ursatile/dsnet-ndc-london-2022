
using EasyNetQ;
using System;
using Subscriber;
using System.Threading.Tasks;


namespace Publisher {
	class Program {
		static async Task Main(string[] args) {
			var bus = RabbitHutch.CreateBus("amqps://uzvpuvak:xnIzcflkcIHZmkgqe4uA-Jp9rvKgi1pX@rattlesnake.rmq.cloudamqp.com/uzvpuvak");
			int i = 0;
			while (true) {
				Console.WriteLine("Press a key to publish a message!");
				Console.ReadKey();
				var message = new ArchdaysMessage {
					CreatedAt = DateTimeOffset.UtcNow,
					MessageBody = $"This is message {i++}"
				};
				await bus.PubSub.PublishAsync(message);
				Console.WriteLine($"Published message with body {message.MessageBody}");
			}
		}
	}
}
