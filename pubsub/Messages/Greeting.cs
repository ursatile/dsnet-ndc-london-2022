namespace Messages;
public class Greeting
{
    public int Number { get;set;}
    public string MachineName { get; set; } = Environment.MachineName;
    public DateTimeOffset CreatedAt{ get;set; }

    public override string ToString()
    {
        return $"Message {Number} from {MachineName} at {CreatedAt:O}";
    }

}
