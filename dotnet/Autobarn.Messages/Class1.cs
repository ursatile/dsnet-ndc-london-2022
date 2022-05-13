using System;
using System.Net.NetworkInformation;
using AutoMapper;

namespace Autobarn.Messages {

    public class NewVehicleMessage {
        public string Registration { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTimeOffset ListedAt { get; set; }
        public NewVehiclePriceMessage ToNewVehiclePriceMessage(int price, string currencyCode) {
            var result = Automaps.Map(this);
            result.Price = price;
            result.CurrencyCode = currencyCode;
            return result;
        }
    }

    public class NewVehiclePriceMessage : NewVehicleMessage {
        public int Price { get; set; }
        public string CurrencyCode { get; set; }

    }

    public class Automaps {
        private static readonly Mapper mapper;
        static Automaps() {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NewVehicleMessage, NewVehiclePriceMessage>());
            mapper = new Mapper(config);
        }

        public static NewVehiclePriceMessage Map(NewVehicleMessage nvm)
            => mapper.Map<NewVehiclePriceMessage>(nvm);
    }


}
