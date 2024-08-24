// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;

[CliCommand(Description = "Get project settings", Name = "setting",
    Parent = typeof(AppveyorCommand.ProjectCommand))]
public class GetSetting : AppveyorCommandBase
{
    protected override string Title => "Get project settings ...";
    protected override string Tag => "project settings";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));

        var result = await apiManager.GetProjectSettingsAsync(Slug, ct).ConfigureAwait(false);
        return result;
    }
}
#nullable restore

