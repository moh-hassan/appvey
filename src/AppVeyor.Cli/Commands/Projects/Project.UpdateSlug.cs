// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

using Api;

#nullable disable

[CliCommand(Description = "Update project slug",Name = "slug",
    Parent = typeof(AppveyorCommand.ProjectCommand.UpdateCommand))]
public class SlugCommand : AppveyorCommandBase
{
    protected override string Title => "Update project slug ...";
    protected override string Tag => "project update slug";

    [CliOption(Description = "Json file that contain project update", Required = true,
        ValidationRules = CliValidationRules.ExistingFile)]
    public FileInfo Json { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager is null) throw new ArgumentNullException(nameof(apiManager));
        var result = await apiManager.UpdateProjectAsync(Json, ct);
        return result;
    }
}

#nullable restore
