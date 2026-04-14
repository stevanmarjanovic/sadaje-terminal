using System;
using System.Collections.Generic;
using System.Linq;

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
    private string HorizontalContentLine => new string(Horizontal, Width - 2);
    private string TopLine => Color.RenderForeground(Corner.UpperLeft + HorizontalContentLine + Corner.UpperRight);
    private string BottomLine => Color.RenderForeground(Corner.LowerLeft + HorizontalContentLine + Corner.LowerRight);
    private string EmptyLine => Color.RenderForeground(VerticalRendered + new string(' ', Width - 2) + VerticalRendered);
    private string HorizontalRendered => Color.RenderForeground(Horizontal.ToString());
    private string VerticalRendered => Color.RenderForeground(Vertical.ToString());

    // Border color
    public AnsiColor Color { get; init; } = new();
    
    // Lines and box width
    private int Width => ContentWidth + HorizontalMargin * 2 + 2;
    private int ContentWidth => Content.Max(line => line.Sum(token => token.Value.Length * 2 - 1) + line.Count - 1);
    public required List<List<Token>> Content { get; init; }

    // Defining margins
    public int Margin { get; init; }
    private int VerticalMargin => Margin;
    private int HorizontalMargin => 1 + (Margin * 2 > 0 ? Margin * 2 : 0);
    private string HorizontalMarginRendered => new(' ', HorizontalMargin);

    public decimal CalculateMargin(string text)
    {
        var availableWidth = Width - text.Length;
        return (decimal)availableWidth / 2;
    }

    public List<string> Render()
    {
        List<string> output = [ TopLine ]; // Top line

        // Top margin
        for (var i = 0; i < VerticalMargin; i++)
            output.Add(EmptyLine);

        // Content
        foreach (var line in Content)
        {
            var content = new List<string>();
            foreach (var token in line)
            {
                var tokenText = string.Join(" ", token.Value.ToCharArray());
                var coloredTextAccent = Color.RenderForeground(tokenText);
                var coloredTextMuted = Color.RenderForegroundMuted(tokenText);
                content.Add(token.Accent ? coloredTextAccent : coloredTextMuted);
            }
            output.Add(VerticalRendered + HorizontalMarginRendered + string.Join(" ", content) + HorizontalMarginRendered + VerticalRendered);
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