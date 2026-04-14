using System;
using System.Collections.Generic;
using System.Linq;
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
                Console.WriteLine("  --dark              Set dark mode");
                Console.WriteLine("  --rounded           Make box corners rounded");
                Console.WriteLine("  --color=<color>     Set the color of the text (black, blue, green, cyan, red, magenta, yellow, white)");
                Console.WriteLine("  --margin=<margin>   Set margin between border and content (this is padding actually)");
                Console.WriteLine("  --hide-time         Hide the current time");
                Console.WriteLine("  --hide-copyright    Hide the copyright notice");
                Console.WriteLine("  --debug             Show some debug data");
                Console.WriteLine("  --help              Show this help message");
                return;
            }

            #region ARGUMENTS

            var showTime = args.All(arg => arg != "--hide-time");
            var showCopyright = args.All(arg => arg != "--hide-copyright");
            var dark = args.Any(arg => arg == "--dark");
            var debug = args.Any(arg => arg == "--debug");
            var rounded = args.Any(arg => arg == "--rounded");

            var boxMargin = 0;
            if (args.Any(arg => arg.StartsWith("--margin=")))
            {
                var selectedMargin = args
                    .Where(arg => arg.StartsWith("--margin="))
                    .Select(arg => arg.Split("=").Last())
                    .FirstOrDefault();

                bool validMargin = int.TryParse(selectedMargin, out boxMargin);

                if (!validMargin)
                    boxMargin = 0;
            }

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
                IsBright = dark,
            };
            var mutedColor = new AnsiColor
            {
                ColorCode = dark ? AnsiColorCodes.Black : AnsiColorCodes.White,
                IsBright = dark
            };

            #endregion

            #region TIME PARSING

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

            List<string> allowedTags = ["NOW"];

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

            #endregion

            if (debug)
                Console.WriteLine($"WORDED: {string.Join(' ', allowedTags)}");

            foreach (var tokenList in Tokens)
            {
                foreach (var token in tokenList)
                {
                    token.Accent = token.Tags.Any(allowedTags.Contains);
                }
            }

            var box = new Box
            {
                Content = Tokens,
                Rounded = rounded,
                Color = color,
                Margin = boxMargin,
            };
            
            // Render tokens
            if (showTime)
            {
                var timeText = now.ToString("hh:mm");
                var timeMargin = box.CalculateMargin(timeText);

                var time = new string(' ', (int)Math.Floor(timeMargin)) + 
                           timeText + 
                           new string(' ', (int)Math.Ceiling(timeMargin));
                Console.WriteLine(color.RenderForegroundMuted(time));
            }

            var outputLines = box.Render();
            foreach (var line in outputLines)
                Console.WriteLine(line);

            if (showCopyright)
            {
                var copyrightMargin = box.CalculateMargin(Copyright);

                var copyrightText = new string(' ', (int)Math.Floor(copyrightMargin)) +
                                    Copyright +
                                    new string(' ', (int)Math.Ceiling(copyrightMargin));
                Console.WriteLine(color.RenderForegroundMuted(copyrightText));
            }
        }
    }
}
