namespace SadaJeTerminal.Domain;

public class ConsoleLine
{
    public ConsoleLine(List<ConsoleText> texts, bool spaced = true)
    {
        Spaced = spaced;

        if (!Spaced)
            Texts = texts;
        else
        {
            foreach (var text in texts)
            {
                text.Text = string.Join(Space, text.Text.ToCharArray());
            }
            Texts = texts;
        }
    }

    public List<ConsoleText> Texts { get; set; } = [];
    public bool Spaced = false;

    private const string Space = " ";
    private const string WideSpace = "   ";
    public string RenderedSpace => Spaced ? WideSpace : Space;
    public int RenderedWidth => Texts.Sum(text => text.RenderedWidth) + RenderedSpace.Length * (Texts.Count - 1);
    
    public string Render()
    {
        var output = string.Join(RenderedSpace, Texts.Select(text => text.Render()));
        return output;
    }
}