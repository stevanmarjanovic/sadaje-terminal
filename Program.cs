using System.Diagnostics;

namespace SadaJeTerminal
{
    internal class Token
    {
        public required string Value { get; init; }
        public List<string> Tags { get; init; } = [];
    }

    internal class ConsoleText(string text, int width)
    {
        public string Text { get; set; } = text;
        public int Width { get; set; } = width;

        public ConsoleText(string text) : this(text, text.Length) { }

        public string Bold => $"\x1b[1m{Text}\x1b[0m";
        public string Center => $"{new string(' ', (Width - Text.Length) / 2)}{Text}";
    }

    internal class Program
    {
        private static List<List<Token>> tokens = [
           [
                new Token { Value = "CX" },
                new Token { Value = "SADA", Tags = [ "NOW" ] },
                new Token { Value = "L" },
                new Token { Value = "JE", Tags = ["NOW"] },
                new Token { Value = "AKL" },
            ],
            [
                new Token { Value = "DVA", Tags = [ "P2", "P20", "P25" ] },
                new Token { Value = "DESET", Tags = [ "P10", "P20", "P25" ] },
                new Token { Value = "POLA", Tags = [ "HALF" ] }
            ],
            [
                new Token { Value = "R" },
                new Token { Value = "PET", Tags = [ "P5", "P15", "P25" ] },
                new Token { Value = "NAEST", Tags = [ "P15" ] },
                new Token { Value = "S" },
                new Token { Value = "DO", Tags = [ "UNTIL" ] },
            ],
            [
                new Token { Value = "G" },
                new Token { Value = "JEDAN", Tags = [ "C1", "C11" ] },
                new Token { Value = "AES", Tags = [ "C11" ] },
                new Token { Value = "T", Tags = [ "C3", "C11" ] },
                new Token { Value = "RI", Tags = [ "C3" ] },
            ],
            [
                new Token { Value = "SEDAM", Tags = [ "C7" ] },
                new Token { Value = "ČETIRI", Tags = [ "C4" ] },
                new Token { Value = "H" },
            ],
            [
                new Token { Value = "DVA", Tags = [ "C2", "C12" ] },
                new Token { Value = "NAEST", Tags = [ "C12" ] },
                new Token { Value = "ŠEST", Tags = [ "C6" ] },
            ],
            [
                new Token { Value = "DEVET", Tags = [ "C9" ] },
                new Token { Value = "DE", Tags = [ "C10" ] },
                new Token { Value = "S", Tags = [ "C10", "HOURGS" ] },
                new Token { Value = "ET", Tags = [ "C10" ] },
                new Token { Value = "BM" },
            ],
            [
                new Token { Value = "F" },
                new Token { Value = "PET", Tags = [ "C5" ] },
                new Token { Value = "Z" },
                new Token { Value = "OS", Tags = [ "C8" ] },
                new Token { Value = "A", Tags = [ "C8", "HOURGS" ] },
                new Token { Value = "M", Tags = [ "C8" ] },
                new Token { Value = "A" },
                new Token { Value = "I", Tags = [ "AND" ] },
                new Token { Value = "S" },
            ],
            [
                new Token { Value = "DVA", Tags = [ "S2", "S20", "S25" ] },
                new Token { Value = "DESE", Tags = [ "S10", "S20", "S25" ] },
                new Token { Value = "T", Tags = [ "S10", "S20", "S25", "HOURGS" ] },
                new Token { Value = "SAT", Tags = [ "HOURGP", "HOURNS" ] },
                new Token { Value = "I", Tags = [ "HOURGP" ] },
            ],
            [
                new Token { Value = "RSV" },
                new Token { Value = "PET", Tags = [ "S5", "S15", "S25" ] },
                new Token { Value = "N", Tags = [ "S15" ] },
                new Token { Value = "A", Tags = [ "S15", "HOURGS" ] },
                new Token { Value = "EST", Tags = [ "S15" ] },
                new Token { Value = "L" },
            ]
        ];
        private static int Width = 12 + 11; // 12 characters per line + 11 spaces between them
        private static int BoxWidth = Width + 4; // 2 for borders and 2 for padding
        private static int Height = 10; // 10 lines

        private static void Write(string text, bool newLine = false, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            if (args.Any(arg => arg == "--help"))
            {
                Console.WriteLine("Usage: sadaje [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --hide-time         Hide the current time");
                Console.WriteLine("  --hide-copyright    Hide the copyright notice");
                Console.WriteLine("  --help              Show this help message");
                Console.WriteLine("  --color=<color>     Set the color of the text (black, darkblue, darkgreen, darkcyan, darkred, darkmagenta, darkyellow, gray, darkgray, blue, green, cyan, red, magenta, yellow, white)");
                return;
            }
            bool showTime = !args.Any(arg => arg == "--hide-time");
            bool showCopyright = !args.Any(arg => arg == "--hide-copyright");
            ConsoleColor color = ConsoleColor.White;
            if (args.Any(arg => arg.StartsWith("--color=")))
            {
                string colorArg = args.First(arg => arg.StartsWith("--color=")).Split('=')[1].ToLower();
                color = colorArg switch
                {
                    "black" => ConsoleColor.Black,
                    "darkblue" => ConsoleColor.DarkBlue,
                    "darkgreen" => ConsoleColor.DarkGreen,
                    "darkcyan" => ConsoleColor.DarkCyan,
                    "darkred" => ConsoleColor.DarkRed,
                    "darkmagenta" => ConsoleColor.DarkMagenta,
                    "darkyellow" => ConsoleColor.DarkYellow,
                    "gray" => ConsoleColor.Gray,
                    "darkgray" => ConsoleColor.DarkGray,
                    "blue" => ConsoleColor.Blue,
                    "green" => ConsoleColor.Green,
                    "cyan" => ConsoleColor.Cyan,
                    "red" => ConsoleColor.Red,
                    "magenta" => ConsoleColor.Magenta,
                    "yellow" => ConsoleColor.Yellow,
                    "white" => ConsoleColor.White,
                    _ => ConsoleColor.White
                };
            }

            var now = DateTime.Now;
            var hour = now.Hour;
            var minute = now.Minute;

            // Round minutes to closest 5
            minute = minute % 5 > 2 ? minute + (5 - minute % 5) : minute - minute % 5;

            // Convert to 12 hour format
            hour = hour > 12 ? hour - 12 : hour;

            List<string> allowedTags = [ "NOW" ];

            // Determine which tags to allow based on the current time
            if (minute == 0)
            {
                allowedTags.Add($"C{hour}");
                string hourTag;
                if (hour > 5)
                    hourTag = "HOURGP";
                else if (hour > 1)
                    hourTag = "HOURGS";
                else
                    hourTag = "HOURNS";
                allowedTags.Add(hourTag);
            }
            else if (minute == 30)
            {
                allowedTags.Add("HALF");
                allowedTags.Add($"C{hour + 1}");
            }
            else if (minute < 30)
            {
                allowedTags.Add($"C{hour}");
                allowedTags.Add("AND");
                allowedTags.Add($"S{minute}");
            }
            else
            {
                allowedTags.Add($"P{60 - minute}");
                allowedTags.Add("UNTIL");
                allowedTags.Add($"C{hour + 1}");
            }

            // Render tokens
            if (showTime)
            {
                Write(
                    new ConsoleText(now.ToString("hh:mm"), BoxWidth).Center,
                    true, ConsoleColor.DarkGray
                );
            }
                
            Write(
                "┌" + new string('─', Width + 2) + "┐",
                true, color
            );
            foreach (var line in tokens)
            {
                Write("│ ", false, color);
                foreach (var token in line)
                {
                    string spacedToken = string.Join(" ", token.Value.ToCharArray());

                    if (token.Tags.Any(tag => allowedTags.Contains(tag)))
                        Write(spacedToken, false, color);
                    else
                        Write(spacedToken, false, ConsoleColor.DarkGray);

                    Console.Write(" ");
                }
                Write("│", true, color);
            }
            Write("└" + new string('─', Width+2) + "┘", true, color);

            if (showCopyright)
            {
                Write(
                    new ConsoleText("(c) Stevan Marjanović", BoxWidth).Center,
                    true, ConsoleColor.DarkGray
                );
            }
        }
    }
}
