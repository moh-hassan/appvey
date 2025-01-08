// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;

[CliCommand(
    Description = "Update project build number.\n" +
    "Example 1:\n" +
    "\tappvey project update build-number -a my-account -t my-token -s my-project 30\n" +
    "Example 2:\n" +
    "\tappvey project update bn -a my-account -t my-token -s my-project 30",
    Name = "build-number",
    Aliases = ["bn"],
    Parent = typeof(AppveyorCommand.ProjectCommand.UpdateCommand))]
public class UpdateBuildNumber : AppveyorCommandBase
{
    protected override string Title => "Update project build number ...";
    protected override string Tag => "project update build-number";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliArgument(Description = "Next Build Number", Required = true)]
    public int BuildNumber { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));
        var result = await apiManager
            .UpdateProjectBuildNumberAsync(Slug, BuildNumber, WhatIf, ct);
        return result;
    }
}

#nullable restore
