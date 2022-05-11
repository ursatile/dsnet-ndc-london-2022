using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Autobarn.Notifier {

	class Program {
		private const string BUS_CONNECTION_STRING =
			"Endpoint=sb://autobarn-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=pwPU/2tHi51k0nJs7wnhu+hPa3kr2N8JdusMJfFdFE0=";

		private const string TOPIC_NAME = "autobarn-new-vehicle-topic";
		private const string SUBSCRIPTION_NAME = "autobarn-notifier-subscription";

		static async Task Main(string[] args) {
			Console.WriteLine("Waiting for messages:");
			await ReceiveMessagesFromSubscriptionAsync();
		}

		static async Task ReceiveMessagesFromSubscriptionAsync() {
			await using ServiceBusClient client = new ServiceBusClient(BUS_CONNECTION_STRING);
			var processor = client.CreateProcessor(TOPIC_NAME, SUBSCRIPTION_NAME, new ServiceBusProcessorOptions());

			// add handler to process messages
			processor.ProcessMessageAsync += MessageHandler;

			// add handler to process any errors
			processor.ProcessErrorAsync += ErrorHandler;

			// start processing 
			await processor.StartProcessingAsync();

			Console.WriteLine("Wait for a minute and then press any key to end the processing");
			Console.ReadKey();

			// stop processing 
			Console.WriteLine("\nStopping the receiver...");
			await processor.StopProcessingAsync();
			Console.WriteLine("Stopped receiving messages");
		}

        static async Task MessageHandler(ProcessMessageEventArgs args) {
			string body = args.Message.Body.ToString();
			Console.WriteLine($"Received: {body} from subscription: {SUBSCRIPTION_NAME}");

			// complete the message. messages is deleted from the queue. 
			await args.CompleteMessageAsync(args.Message);
		}

		static Task ErrorHandler(ProcessErrorEventArgs args) {
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		}
    }
}
