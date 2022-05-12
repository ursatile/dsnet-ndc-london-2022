using EasyNetQ;
using Messages;

const string AMQP = "amqps://kptbknvz:a5BBGuE0YJqtgjGHp7ChJei5JMs0xGRX@cougar.rmq.cloudamqp.com/kptbknvz";
using (var bus = RabbitHutch.CreateBus(AMQP)) {
    bus.PubSub.Subscribe<Greeting>("dylan-clean-subscriber", HandleGreeting);
    Console.WriteLine("Listening for messages - press Enter to quit");
    Console.ReadLine();
    Console.WriteLine("Exited cleanly that time!");
}

void HandleGreeting(Greeting greeting)
{
    if (greeting.Number % 5 == 0) throw new Exception("FIVE IS THE BAD NUMBER!");
    Console.WriteLine(greeting);
}