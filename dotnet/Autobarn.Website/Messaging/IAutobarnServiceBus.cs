using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autobarn.Website.Models;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Autobarn.Website.Messaging {
	public interface IAutobarnServiceBus {
		void PublishNewVehicleNotification(VehicleDto dto);
	}

	public class AutobarnAzureServiceBus : IAutobarnServiceBus {
		private readonly ServiceBusSender sender;

		public AutobarnAzureServiceBus(ServiceBusClient client, string topic) {
			this.sender = client.CreateSender(topic);
		}
		public async void PublishNewVehicleNotification(VehicleDto dto) {
			var json = JsonConvert.SerializeObject(dto);
			var message = new ServiceBusMessage(json);
			await sender.SendMessageAsync(message);
		}
	}
}
