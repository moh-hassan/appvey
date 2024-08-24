// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

internal static class InputHelper
{
    public static string? ProcessToken(string? token)
    {
        if (token == null) return token;
        token = token.Trim();
        return token == "-" ? ReadStdio() : token;
    }

    private static string ReadStdio()
    {
        return TryReadInputPipe(out var input)
            ? input
            : ReadPassword();
    }

    private static string ReadPassword()
    {
        Console.Write("Enter token: ");
        var password = string.Empty;
        var info = Console.ReadKey(true);
        while (info.Key != ConsoleKey.Enter)
        {
            if (info.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                var pos = Console.CursorLeft;
                Console.SetCursorPosition(pos - 1, Console.CursorTop);
                Console.Write(" ");
                Console.SetCursorPosition(pos - 1, Console.CursorTop);
            }
            else if (info.Key != ConsoleKey.Backspace)
            {
                Console.Write("*");
                password += info.KeyChar;
            }

            info = Console.ReadKey(true);
        }

        Console.WriteLine();
        return password;
    }

    private static bool TryReadInputPipe(out string input)
    {
        if (Console.IsInputRedirected)
        {
            WriteInfo("Input is redirected, reading token from input pipeline.");
            using var reader = new StreamReader(Console.OpenStandardInput(8192));
            input = reader.ReadToEnd().Trim();
            return true;
        }

        input = string.Empty;
        return false;
    }
}
