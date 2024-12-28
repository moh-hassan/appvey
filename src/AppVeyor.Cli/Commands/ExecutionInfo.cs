// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

using System.Text;

public static class ExecutionInfo
{
    public static string? Title { get; set; }
    public static string? Tag { get; set; }
    public static string? Request { get; set; }

    public static void Clear()
    {
        Title = null;
        Tag = null;
        Request = null;
    }

    public static StringBuilder Show(bool verbose)
    {
        if (!verbose) return new StringBuilder();
        var sb = new StringBuilder();
        sb.AppendLine("Execution information:");
        sb.AppendLine($"Title: {Title}");
        sb.AppendLine($"Tag: {Tag}");
        if (Request is { } request)
            sb.AppendLine($"Request: {request}");
        WriteInfo(sb.ToString());
        return sb;
    }
}
