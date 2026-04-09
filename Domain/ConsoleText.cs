using System.Text.Encodings.Web;

namespace SadaJeTerminal.Domain;

public class ConsoleText(string text)
{
    public string Text { get; set; } = text;
    public AnsiColor Color { get; init; } = new();

    public int RenderedWidth => Text.Length;

    public string Render()
    {
        var output = Color.EscapeSequance + Text + Color.ResetSequence;

        return output;
    }
}