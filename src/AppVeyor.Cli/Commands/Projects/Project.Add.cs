// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;
using AppVeyor.RestApi.Extensions;

[CliCommand(
    Description = "Add project.\n" +
    "Example 1: Add project for github\n" +
    "\tappvey project add -a my-account -t my-token my-account/my-project\n" +
     "Example 2: Add project for gitlab\n" +
    "\tappvey project add -a my-account -t my-token -p gitlab my-account/my-project\n",
    Parent = typeof(AppveyorCommand.ProjectCommand))]
public class Add : AppveyorCommandBase
{
    protected override string Title => "Add project ...";

    protected override string Tag => "project add";

    [CliOption(Description = "Repository Provider", Name = "--provider")]
    public RepositoryProvider RepositoryProvider { get; set; } = RepositoryProvider.gitHub;

    [CliArgument(Description = "Repository Name in the form: account/repo_name",
        HelpName = "account/repo_name", Required = true)]
    public string RepositoryName { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));

        var result = await apiManager
            .AddProjectAsync(RepositoryProvider.Enum2String(), RepositoryName, WhatIf, ct);
        return result;
    }
}
