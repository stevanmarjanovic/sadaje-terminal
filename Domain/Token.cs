using System.Collections.Generic;

namespace SadaJeTerminal.Domain;

public class Token
{
    public required string Value { get; init; }
    public List<string> Tags { get; init; } = [];
    public bool Accent { get; set; }
}