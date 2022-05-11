namespace Autobarn.Messages {
	public class NewVehiclePriceMessage : NewVehicleMessage {
		public int Price { get; set; }
		public string CurrencyCode { get; set; }
	}

	public static class MessageExtensions {
		public static NewVehiclePriceMessage ToNewVehiclePriceMessage(this NewVehicleMessage incomingMessage, int price,
			string currencyCode) {
			return new NewVehiclePriceMessage {
				Manufacturer = incomingMessage.Manufacturer,
				ModelCode = incomingMessage.ModelCode,
				Color = incomingMessage.Color,
				ModelName = incomingMessage.ModelName,
				Registration = incomingMessage.Registration,
				Year = incomingMessage.Year,
				CurrencyCode = currencyCode,
				Price = price
			};
		}
	}
}