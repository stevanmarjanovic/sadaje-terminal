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
    public AnsiColorCodes ColorCode { get; init; } = AnsiColorCodes.Default;
    public bool IsBackground { get; init; } = false;
    public bool IsBright { get; init; } = true;
    public bool IsDefault => ColorCode == AnsiColorCodes.Default;

    public string Foreground => $"3{(int)ColorCode}";
    public string Background => $"4{(int)ColorCode}";
    public string ForegroundBright => IsDefault ? Foreground : $"9{(int)ColorCode}";
    public string BackgroundBright => IsDefault ? Background : $"10{(int)ColorCode}";

    private static string WrapSequence(string sequence) => $"\x1b[{sequence}m";

    public string EscapeSequance => (IsBackground, IsBright) switch
    {
        (true,  true)  => WrapSequence(BackgroundBright),
        (true,  false) => WrapSequence(Background),
        (false, true)  => WrapSequence(ForegroundBright),
        (false, false) => WrapSequence(Foreground)
    };

    public string ResetSequence => WrapSequence(IsBackground ? "49" : "39");
}