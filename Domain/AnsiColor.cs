namespace SadaJeTerminal.Domain;

public enum AnsiColorCodes
{
    Black,
    Red,
    Green,
    Yellow,
    Blue,
    Magenta,
    Cyan,
    White,
    Default = 9
}

public class AnsiColor
{
    public AnsiColor() { }

    public AnsiColor(AnsiColorCodes colorCode)
    {
        ColorCode = colorCode;
    }
    public AnsiColor(AnsiColor color)
    {
        ColorCode = color.ColorCode;
        IsBackground = color.IsBackground;
        IsBright = color.IsBright;
    }
    
    public AnsiColorCodes ColorCode { get; init; } = AnsiColorCodes.Default;
    public bool IsBackground { get; set; } = false;
    public bool IsBright { get; set; } = true;
    private bool IsDefault => ColorCode == AnsiColorCodes.Default;

    private string Foreground => $"3{(int)ColorCode}";
    private string Background => $"4{(int)ColorCode}";
    private string ForegroundBright => IsDefault ? Foreground : $"9{(int)ColorCode}";
    private string BackgroundBright => IsDefault ? Background : $"10{(int)ColorCode}";

    private static string WrapSequence(string sequence) => $"\e[{sequence}m";

    private string EscapeForegroundSequence => IsBright ? WrapSequence(ForegroundBright) : WrapSequence(Foreground);
    private string EscapeForegroundMutedSequence => IsBright ? WrapSequence("90") : WrapSequence("37");
    
    private string EscapeBackgroundSequence => IsBright ? WrapSequence(BackgroundBright) : WrapSequence(Background);
    private string ResetForegroundSequence => WrapSequence("39");
    private string ResetBackgroundSequence => WrapSequence("49");

    public string RenderForeground(string text)
    {
        return EscapeForegroundSequence + text + ResetForegroundSequence;
    }

    public string RenderForegroundMuted(string text)
    {
        return EscapeForegroundMutedSequence + text + ResetForegroundSequence;
    }

    public string RenderBackground(string text)
    {
        return EscapeBackgroundSequence + text + ResetBackgroundSequence;
    }
}