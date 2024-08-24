// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

internal class DummyConsole : IConsole
{
    public void WriteLine(string text)
    {
    }

    public void Write(string text)
    {
    }

    public void ResetColor()
    {
    }

    public ConsoleColor ForegroundColor
    {
        get => Console.ForegroundColor;
        set => Console.ForegroundColor = value;
    }
}
