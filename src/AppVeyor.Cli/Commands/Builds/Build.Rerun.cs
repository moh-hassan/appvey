// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Build;

#nullable disable
using System.Net;
using Api;

[CliCommand(
    Description = "Re-run build.\n" +
    "Example:\n" +
    "\tappvey build rerun -a my-account -t my-token -s my-project 1234",
    Parent = typeof(AppveyorCommand.BuildCommand))]
public class Rerun : AppveyorCommandBase, IBrowse
{
    protected override string Title => "Re-run build ...";
    protected override string Tag => "build rerun";

    [CliOption(Description = "False (default value) for full build re-run. Set it to True to rerun only failed or cancelled jobs in multi jobs build.")]
    public bool Incomplete { get; set; }

    [CliOption(Description = "A Slug is not required. However, you might need to navigate to the build page to find the information of the build during run.", Required = false)]
    public string Slug { get; set; }

    public bool Browse { get; set; }

    [CliArgument(Description = "BuildId", Required = true)]
    public string BuildId { get; set; }
    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));

        var result = await apiManager
            .ReRunBuildCommitAsync(BuildId, Incomplete, WhatIf, ct);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            WriteLine("Failed to start the build");
        }
        else
        {
            WriteLine("Build started successfully");
            PrintBuildReport(result, Browse, Slug);
        }

        return result;
    }
}
#nullable restore


