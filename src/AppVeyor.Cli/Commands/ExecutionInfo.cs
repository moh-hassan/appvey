// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;
#nullable disable
using System.Text;

public class ExecutionInfo
{
    public string Title { get; set; }
    public string Tag { get; set; }
    public string Request { get; set; }

    public StringBuilder Show(bool verbose)
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
#nullable restore
