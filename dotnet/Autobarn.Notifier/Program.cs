using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Autobarn.Notifier {
	class Program {
		const string SIGNALR_HUB_URL = "https://workshop.ursatile.com:5001/hub";
		private static HubConnection hub;

		static async Task Main(string[] args) {
			hub = new HubConnectionBuilder().WithUrl(SIGNALR_HUB_URL).Build();
			await hub.StartAsync();
			Console.WriteLine("Hub started!");
			Console.WriteLine("Press any key to send a message (Ctrl-C to quit)");
			var i = 0;
			while (true) {
				Console.ReadKey(false);
				// The first argument here needs to match the name 
				// of a method defined in the Signalr Hub
				var message = $"Message #{i++} from Autobarn.Notifier";
				await hub.SendAsync("NotifyWebUsers", "Autobarn.Notifier", message);
				Console.WriteLine($"Sent: {message}");
			}
		}
	}
}