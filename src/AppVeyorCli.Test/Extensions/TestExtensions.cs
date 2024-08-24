// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test.Extensions;

using System.CommandLine.Parsing;
using System.Text.RegularExpressions;

internal static class TestHelper
{
    public static string RemoveSpaces(this string str)
    {
        var pattern = @"\s+";
        return Regex.Replace(str.Trim(), pattern, "");
    }

    public static FileInfo WriteToTempFile(this string content)
    {
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, content);
        return new FileInfo(tempFile);
    }

    public static string[] SplitArgs(this string input) => CliParser.SplitCommandLine(input).ToArray();
}
