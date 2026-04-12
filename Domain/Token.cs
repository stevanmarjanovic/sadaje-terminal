namespace SadaJeTerminal.Domain;

public class Token
{
    public required string Value { get; init; }
    public List<string> Tags { get; init; } = [];
}