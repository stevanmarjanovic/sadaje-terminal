namespace SadaJeTerminal.Domain;

public class Corner {
    public bool Rounded { get; set; } = false;
    public char UpperLeft => Rounded ? '╭' : '┌';
    public char UpperRight => Rounded ? '╮' : '┐';
    public char LowerLeft => Rounded ? '╰' : '└';
    public char LowerRight => Rounded ? '╯' : '┘';
}

public class Box {
    // Constants
    public const char Vertical = '|';
    public const char Horizontal = '─';
    
    // Defining corners
    public bool Rounded { get; set; } = false;
    public Corner Corner => new() { Rounded = Rounded };
    
    // Lines and box width
    public int Width => Lines.Max(line => line.RenderedWidth);
    public required List<ConsoleLine> Lines { get; init; }

    // Defining margins
    public int Margin { get; set; } = 0;
    public int VerticalMargin => Margin;
    public int HorizontalMargin => 1 + (Margin * 2 > 0 ? Margin * 2 : 0);

    public List<string> GetOutput()
    {
        var output = new List<string>();

        // Empty line
        var emptyLine = Vertical + new string(' ', Width + HorizontalMargin*2) + Vertical;

        // Top line
        var horizontalLine = new string(Horizontal, Width + HorizontalMargin*2);
        output.Add(Corner.UpperLeft + horizontalLine + Corner.UpperRight);

        // Top margin
        for (var i = 0; i < VerticalMargin; i++)
            output.Add(emptyLine);

        // Content
        foreach (var line in Lines)
        {
            var content = new string(' ', HorizontalMargin) + line.Render() + new string(' ', HorizontalMargin + Width - line.RenderedWidth);
            output.Add(Vertical + content + Vertical);
        }

        // Bottom margin
        for (var i = 0; i < VerticalMargin; i++)
            output.Add(emptyLine);

        // Bottom line
        output.Add(Corner.LowerLeft + horizontalLine + Corner.LowerRight);
        
        return output;
    }
}