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
    private string MutedBright = "98";
    private string Muted = "37";

    private static string WrapSequence(string sequence) => $"\e[{sequence}m";

    public string EscapeSequence => (IsBackground, IsBright) switch
    {
        (true,  true)  => WrapSequence(BackgroundBright),
        (true,  false) => WrapSequence(Background),
        (false, true)  => WrapSequence(ForegroundBright),
        (false, false) => WrapSequence(Foreground)
    };

    public string ResetSequence => WrapSequence(IsBackground ? "49" : "39");

    public string Render(string text)
    {
        return EscapeSequence + text + ResetSequence;
    }

    public AnsiColor Filled()
    {
        return new AnsiColor(this)
        {
            IsBackground = true
        };
    }
}