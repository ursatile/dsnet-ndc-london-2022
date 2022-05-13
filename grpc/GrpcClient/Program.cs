using System.Reflection.Emit;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeter;

using var channel = GrpcChannel.ForAddress("https://localhost:7140");
var client = new Greeter.GreeterClient(channel);

while (true) {
    Console.WriteLine("1 = British English; 2 = US English; 3 = Hebrew; 4 = Romanian, 5 = German, 6 = Swedish, 7 - Swiss French");
    if (Int32.TryParse(Console.ReadKey(true).KeyChar.ToString(), out var number)) {
        var language = number switch {
            2 => "en-US",
            3 => "he-IL",
            4 => "ro-RO",
            5 => "de-DE",
            6 => "sv-SE",
            7 => "fr-CH",
            _ => "en-GB"
        };

        var request = new HelloRequest {
            Language = language,
            FirstName = "NDC",
            LastName = "London"
        };

        var reply = await client.SayHelloAsync(request);
        Console.WriteLine(reply.Message);
    }
}
