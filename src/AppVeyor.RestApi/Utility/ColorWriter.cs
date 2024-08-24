// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

using System.Text.RegularExpressions;

public static class ColorWriter
{
    private static IConsole s_console = new ConsoleWrapper();

    public static void SetConsole(IConsole console)
    {
        s_console = console;
    }

    public static void WriteSuccess(string text)
    {
        Write(text, ConsoleColor.Green);
    }

    public static void WriteInfo(string text)
    {
        Write(text, ConsoleColor.Cyan);
    }

    public static void WriteWarning(string text)
    {
        Write(text, ConsoleColor.Yellow);
    }

    public static void WriteError(string text)
    {
        Write(text, ConsoleColor.Red);
    }

    public static void WriteVitalError(string text)
    {
        Write(text, ConsoleColor.DarkRed);
    }

    public static void WriteLine(object? text = null)
    {
        if (text == null)
            s_console.WriteLine(string.Empty);
        s_console.WriteLine($"{text}");
        Logger.Log($"{text}");
    }

    public static void Write(string text)
    {
        s_console.Write(text);
        Logger.Log(text);
    }

    private static void Write(string text, ConsoleColor color)
    {
        s_console.ForegroundColor = color;
        s_console.WriteLine(text);
        s_console.ResetColor();
        Logger.Log(text);
    }

    //parse: "[red] hi world [\] [green] thank you [\]"
    public static void WriteColoredText(string text)
    {
        var pattern = @"\[(?<color>\w+\s*)\](?<text>.*?\s*)\[\\\]";
        var regex = new Regex(pattern);

        var matches = regex.Matches(text);
        foreach (Match match in matches)
        {
            var color = match.Groups["color"].Value;
            var extractedText = match.Groups["text"].Value;
            var consoleColor = GetColor(color);

            WriteSegment(extractedText, consoleColor);
        }

        WriteLine("");
    }

    private static ConsoleColor GetColor(string color)
    {
        // Return the corresponding ConsoleColor based on the input color string
        return color.ToLower() switch
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
            _ => ConsoleColor.Gray, // Default color if no match is found
        };
    }

    private static void WriteSegment(string text, ConsoleColor color)
    {
        s_console.ForegroundColor = color;
        s_console.Write(text);
        s_console.ResetColor();
        Logger.Log(text);
    }
}
