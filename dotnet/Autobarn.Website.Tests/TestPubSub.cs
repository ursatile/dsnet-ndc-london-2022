using EasyNetQ;
using EasyNetQ.Internals;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Autobarn.Website.Tests {
	public class TestPubSub : IPubSub {
		public Task PublishAsync<T>(T message, Action<IPublishConfiguration> configure, CancellationToken cancellationToken = new CancellationToken()) {
			return Task.CompletedTask;
		}

		public AwaitableDisposable<ISubscriptionResult> SubscribeAsync<T>(string subscriptionId, Func<T, CancellationToken, Task> onMessage, Action<ISubscriptionConfiguration> configure,
			CancellationToken cancellationToken = new CancellationToken()) {
			throw new NotImplementedException();
		}
	}
}