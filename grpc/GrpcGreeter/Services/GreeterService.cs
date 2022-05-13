using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    private static int count = 0;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        var name = $"{request.FirstName} {request.LastName}";
        var message = request.Language switch {
            "en-GB" => $"Good morning, {name}!",
            "en-US" => $"Howdy, {name}",
            "he-IL" => "בוקר טובֿ",
            "pl-PL" => "Dziendobry, {name}",
            "ro-RO" => $"Neata, {name}!",
            "de-DE" => $"Guten Morgen {name}!",
            "sv-SE" => $"God morgon, {name}!",
            "fr-CH" => $"Bonjour bonjour, {name}!",
            _ => $"Hello {name}"
        };

        return Task.FromResult(new HelloReply {
            Message = $"{count++}: {message}"
        });
    }
}
