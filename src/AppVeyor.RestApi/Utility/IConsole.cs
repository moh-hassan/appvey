// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

public interface IConsole
{
    void WriteLine(string text);
    void Write(string text);
    void ResetColor();
    ConsoleColor ForegroundColor { get; set; }
}
