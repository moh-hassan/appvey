// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;

[CliCommand(Description = "Delete project", Name = "delete",
    Parent = typeof(AppveyorCommand.ProjectCommand))]
public class ProjectDelete : AppveyorCommandBase
{
    protected override string Title => "Delete project ...";
    protected override string Tag => "project delete";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager is null) throw new ArgumentNullException(nameof(apiManager));
        var result = await apiManager.DeleteProjectAsync(Slug, ct);
        return result;
    }
}
#nullable restore

