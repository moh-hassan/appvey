// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.Cli.Commands;

public class ExecutionInfo
{
    public ExecutionInfo()
    {
    }

    public ExecutionInfo(string title, string tag)
    {
        Title = title;
        Tag = tag;
    }

    public string Title { get; set; }
    public string Tag { get; set; }
    public string Request { get; set; }

    public static void Copy(ExecutionInfo source, ExecutionInfo target)
    {
        target.Title = source.Title;
        target.Tag = source.Tag;
        target.Request = source.Request;
    }

    public void Show(bool verbose)
    {
        if (!verbose) return;
        WriteInfo("Execution information:");
        WriteInfo($"Title: {Title}");
        WriteInfo($"Tag: {Tag}");
        WriteInfo($"Request: {Request}");
    }
}
