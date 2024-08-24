// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;
using AppVeyorCli;

[CliCommand(Description = "Get project last branch build",
    Parent = typeof(AppveyorCommand.ProjectCommand))]
public class LastBuild : AppveyorCommandBase
{
    protected override string Title => "Get Project last branch build ...";
    protected override string Tag => "project last-build";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliOption(Description = "Repository branch", Required = false)]
    public string Branch { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));
        var rr = await apiManager.GetProjectLastBranchBuildAsync(Slug, Branch, ct);
        return rr;
    }

    protected override void DisplayResponseResult(ResponseResult result)
    {
        if (!result.IsSuccess) return;
        var summary = result.ProjectBuildByVersion();
        WriteLine("Response summary:");
        WriteLine(summary);
    }
}

#nullable restore
