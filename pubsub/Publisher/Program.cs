using EasyNetQ;
using Messages;
const string AMQP = "amqps://kptbknvz:a5BBGuE0YJqtgjGHp7ChJei5JMs0xGRX@cougar.rmq.cloudamqp.com/kptbknvz";
var bus = RabbitHutch.CreateBus(AMQP);
var count = 0;
while(true) {
    Console.WriteLine("Press any key to send a message!");
    Console.ReadKey(false);
    var greeting = new Greeting {
        CreatedAt = DateTimeOffset.Now,
        Number = count++
    };
    Console.WriteLine($"Sending: {greeting}");
    bus.PubSub.Publish(greeting);
}



