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
    private const char Vertical = '│';
    private const char Horizontal = '─';
    
    // Defining corners
    public bool Rounded { get; set; } = false;
    public Corner Corner => new() { Rounded = Rounded };

    // Defining lines
    private string HorizontalLine => new string(Horizontal, Width - 2);
    private string TopLine => Filled ? Color.Render(new string(' ', Width)) : Color.Render(Corner.UpperLeft + HorizontalLine + Corner.UpperRight);
    private string BottomLine => Filled ? Color.Render(new string(' ', Width)) : Color.Render(Corner.LowerLeft + HorizontalLine + Corner.LowerRight);
    private string EmptyLine => Filled ? Color.Render(new string(' ', Width)) : Color.Render(VerticalRendered + new string(' ', Width - 2) + VerticalRendered);
    private string HorizontalRendered => Color.Render(Horizontal.ToString());
    private string VerticalRendered => Filled ? Color.Render(" ") : Color.Render(Vertical.ToString());

    // Border color
    public AnsiColor Color { get; set; } = new AnsiColor();
    public bool Filled { get; set; } = false;
    
    // Lines and box width
    public int Width => ContentWidth + HorizontalMargin * 2 + 2;
    public int ContentWidth => Lines.Max(line => line.RenderedWidth);
    public required List<ConsoleLine> Lines { get; init; }

    // Defining margins
    public int Margin { get; set; } = 0;
    public int VerticalMargin => Margin;
    public int HorizontalMargin => 1 + (Margin * 2 > 0 ? Margin * 2 : 0);

    private string RenderContent(string content) => Color.Render(content);

    public decimal CalculateMargin(string text)
    {
        var availableWidth = Width - text.Length;
        return (decimal)availableWidth / 2;
    }

    public List<string> Render()
    {
        var output = new List<string>();

        // Top line
        output.Add(TopLine);

        // Top margin
        for (var i = 0; i < VerticalMargin; i++)
            output.Add(EmptyLine);

        // Content
        foreach (var line in Lines)
        {
            var content =
                RenderContent(new string(' ', HorizontalMargin)) +
                line.Render() +
                RenderContent(new string(' ', HorizontalMargin + ContentWidth - line.RenderedWidth));
            output.Add(VerticalRendered + content + VerticalRendered);
        }

        // Bottom margin
        for (var i = 0; i < VerticalMargin; i++)
            output.Add(EmptyLine);

        // Bottom line
        output.Add(BottomLine);
        
        return output;
    }
    
    public override string ToString()
    {
        return string.Join(Environment.NewLine, Render());
    }
}