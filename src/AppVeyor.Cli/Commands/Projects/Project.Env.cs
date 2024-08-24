// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;

[CliCommand(Description = "Get project environment variables", Parent = typeof(AppveyorCommand.ProjectCommand),
        Name = "env")]
public class EnvironmentCommand : AppveyorCommandBase
{
    protected override string Title => "Get project environment variables ...";
    protected override string Tag => "project env";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager is null) throw new ArgumentNullException(nameof(apiManager));
        var result = await apiManager.GetProjectEnvironmentAsync(Slug, ct).ConfigureAwait(false);
        return result;
    }
}
#nullable restore


