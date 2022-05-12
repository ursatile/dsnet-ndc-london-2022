using EasyNetQ;
using Messages;

const string AMQP = "amqps://kptbknvz:a5BBGuE0YJqtgjGHp7ChJei5JMs0xGRX@cougar.rmq.cloudamqp.com/kptbknvz";
var bus = RabbitHutch.CreateBus(AMQP);
bus.PubSub.Subscribe<Greeting>("SUBSCRIBER", greeting => {
    Console.WriteLine(greeting);
});
Console.WriteLine("Listening for messages - press Enter to quit");
Console.ReadLine();

