// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;

[CliCommand(Description = "Add project", Parent = typeof(AppveyorCommand.ProjectCommand))]
public class Add : AppveyorCommandBase
{
    protected override string Title => "Add project ...";

    protected override string Tag => "project add";

    [CliOption(Description = "Repository Provider", Name = "--provider",
        AllowedValues = ["gitHub", "bitBucket", "gitLab"])]
    public string RepositoryProvider { get; set; } = "gitHub";

    [CliArgument(Description = "Repository Name in the form: account/repo", Required = true)]
    public string RepositoryName { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));

        var result = await apiManager.AddProjectAsync(RepositoryProvider, RepositoryName, ct);
        return result;
    }
}

#nullable restore
