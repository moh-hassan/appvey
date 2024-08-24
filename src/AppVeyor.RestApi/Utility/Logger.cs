// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

using System.Text;

public static class Logger
{
    private static StringBuilder s_log = new();
    public static string Text => s_log.ToString();

    public static void Log(string message)
    {
        s_log.AppendLine(message);
    }

    public static void Clear()
    {
        s_log.Clear();
    }

    public static void Save(FileInfo fi)
    {
        File.WriteAllText(fi.FullName, s_log.ToString());
    }

    public static string Print()
    {
        var log = s_log.ToString();
        Console.WriteLine(log);
        return log;
    }
}
