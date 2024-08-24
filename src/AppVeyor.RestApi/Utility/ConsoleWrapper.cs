// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

internal class ConsoleWrapper : IConsole
{
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }

    public void Write(string text)
    {
        Console.Write(text);
    }

    public void ResetColor()
    {
        Console.ResetColor();
    }

    public ConsoleColor ForegroundColor
    {
        get => Console.ForegroundColor;
        set => Console.ForegroundColor = value;
    }
}
