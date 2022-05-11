using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Autobarn.Website.Hubs {
	public class AutobarnHub : Hub {
		public async Task NotifyWebUsers(string user, string message) {
			// The first argument to SendAsync needs to match the string defined in the handler
			// in our client-side JS code.
			await Clients.All.SendAsync("DisplayNotification", user, message);
		}
	}
}
