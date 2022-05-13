using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Controllers.api;
using Autobarn.Website.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Internals;
using Xunit;
using System.Collections;
using Autobarn.Messages;
using Shouldly;

namespace Autobarn.Website.Tests {
    public class ModelsControllerTests {
        [Fact]
        public void POST_Vehicles_Publishes_Message_On_Bus() {
            var db = new FakeDatabase();
            var bus = new FakeBus();
            var c = new ModelsController(db, bus);
            var dto = new VehicleDto() {
                Registration = "OUTATIME",
                ModelCode = "dmc-delorean",
                Year = 1985,
                Color = "Silver"
            };
            var result = c.Post("dmc-delorean", dto);
            var fakePubSub = (FakePubSub) bus.PubSub;
            var bucket = fakePubSub.Bucket;
            bucket.Count.ShouldBe(1);
            var message = (NewVehicleMessage) bucket[0];
            message.Color.ShouldBe("Silver");
        }
    }

    public class FakeDatabase : IAutobarnDatabase {
        public int CountVehicles() {
            throw new System.NotImplementedException();
        }

        public void CreateVehicle(Vehicle vehicle) { }

        public void DeleteVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }

        public Manufacturer FindManufacturer(string code) {
            throw new System.NotImplementedException();
        }

        public Model FindModel(string code) => new Model {
            Name = "test",
            Manufacturer = new Manufacturer {
                Name = "test"
            }
        };

        public Vehicle FindVehicle(string registration) => null;

        public IEnumerable<Manufacturer> ListManufacturers() {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Model> ListModels() {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Vehicle> ListVehicles() {
            throw new System.NotImplementedException();
        }

        public void UpdateVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }
    }

    public class FakeBus : IBus {
        public FakeBus() {
            this.PubSub = new FakePubSub();
        }


        public void Dispose() {
            throw new System.NotImplementedException();
        }

        public IPubSub PubSub { get; }
        public IRpc Rpc { get; }
        public ISendReceive SendReceive { get; }
        public IScheduler Scheduler { get; }
        public IAdvancedBus Advanced { get; }
    }

    public class FakePubSub : IPubSub {
        public ArrayList Bucket { get; set; } = new ArrayList();
        public Task PublishAsync<T>(T message, Action<IPublishConfiguration> configure, CancellationToken cancellationToken = new CancellationToken())
        {
            Bucket.Add(message);
            return Task.CompletedTask;

        }

        public AwaitableDisposable<ISubscriptionResult> SubscribeAsync<T>(string subscriptionId, Func<T, CancellationToken, Task> onMessage, Action<ISubscriptionConfiguration> configure,
            CancellationToken cancellationToken = new CancellationToken()) {
            throw new NotImplementedException();
        }
    }
}
