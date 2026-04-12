using SadaJeTerminal.Data;
using SadaJeTerminal.Domain;

namespace SadaJeTerminal
{
    internal abstract class Program
    {
        private static readonly List<List<Token>> Tokens = TokenData.Tokens;
        private const string Copyright = "(c) Stevan Marjanović";

        private static void Main(string[] args)
        {
            if (args.Any(arg => arg == "--help"))
            {
                Console.WriteLine("Usage: sadaje [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  --hide-time         Hide the current time");
                Console.WriteLine("  --hide-copyright    Hide the copyright notice");
                Console.WriteLine("  --help              Show this help message");
                Console.WriteLine("  --color=<color>     Set the color of the text (black, blue, green, cyan, red, magenta, yellow, white)");
                return;
            }

            var showTime = args.All(arg => arg != "--hide-time");
            var showCopyright = args.All(arg => arg != "--hide-copyright");
            var isDark = args.Any(arg => arg == "--dark");
            var debug = args.Any(arg => arg == "--debug");

            var colorCode = AnsiColorCodes.Default;
            if (args.Any(arg => arg.StartsWith("--color=")))
            {
                var selectedColor = args
                    .Where(arg => arg.StartsWith("--color="))
                    .Select(arg => arg.Split("=").Last())
                    .FirstOrDefault();
                
                Enum.TryParse(selectedColor, true, out colorCode);
            }
            var color = new AnsiColor
            {
                ColorCode = colorCode,
                IsBright = isDark,
            };
            var mutedColor = new AnsiColor
            {
                ColorCode = isDark ? AnsiColorCodes.Black : AnsiColorCodes.White,
                IsBright = isDark,
            };

            var now = DateTime.Now;
            var hour = now.Hour;
            var minute = now.Minute;

            // TODO 20:59 not working
            if (debug)
                Console.WriteLine($"RAW: {hour} {minute}");

            // Round minutes to closest 5
            minute = minute % 5 > 2 ? minute + (5 - minute % 5) : minute - minute % 5;

            // Convert to 12-hour format
            hour = hour > 12 ? hour - 12 : hour;
            
            if (debug)
                Console.WriteLine($"ROUNDED: {hour} {minute}");

            List<string> allowedTags = [ "NOW" ];

            switch (minute)
            {
                // Determine which tags to allow based on the current time
                case 0:
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
                    break;
                }
                case 30:
                    allowedTags.Add("HALF");
                    allowedTags.Add($"C{hour + 1}");
                    break;
                case < 30:
                    allowedTags.Add($"C{hour}");
                    allowedTags.Add("AND");
                    allowedTags.Add($"S{minute}");
                    break;
                default:
                    allowedTags.Add($"P{60 - minute}");
                    allowedTags.Add("UNTIL");
                    allowedTags.Add($"C{hour + 1}");
                    break;
            }
            
            if (debug)
                Console.WriteLine($"WORDED: {string.Join(' ', allowedTags)}");
                
            var consoleLines = new List<ConsoleLine>();
            foreach (var line in Tokens)
            {
                var consoleTexts = new List<ConsoleText>();
                foreach (var token in line)
                {
                    if (token.Tags.Any(allowedTags.Contains))
                        consoleTexts.Add(
                            new ConsoleText(token.Value)
                            {
                                Color = color,
                            }
                        );
                    else
                        consoleTexts.Add(
                            new ConsoleText(token.Value)
                            {
                                Color = mutedColor
                            }
                        );
                }
                consoleLines.Add(new ConsoleLine(consoleTexts));
            }

            var box = new Box
            {
                Lines = consoleLines,
            };
            
            // Render tokens
            if (showTime)
            {
                var timeText = now.ToString("hh:mm");
                var margin = box.CalculateMargin(timeText);
                
                var time = new ConsoleText(
                    new string(' ', (int)Math.Floor(margin)) +
                    timeText +
                    new string(' ', (int)Math.Ceiling(margin))
                ) { Color = mutedColor };
                Console.WriteLine(time);
            }

            var outputLines = box.Render();
            foreach (var line in outputLines)
                Console.WriteLine(line);

            if (showCopyright)
            {
                var margin = box.CalculateMargin(Copyright);
                
                var copyrightText = new ConsoleText(
                    new string(' ', (int)Math.Floor(margin)) +
                    Copyright +
                    new string(' ', (int)Math.Ceiling(margin))
                ) { Color = mutedColor };
                Console.WriteLine(copyrightText);
            }
        }
    }
}
