// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;

[CliCommand(Description = "Get project settings in YAML", Name = "yaml",
    Parent = typeof(AppveyorCommand.ProjectCommand))]
public class GetYamlSetting : AppveyorCommandBase
{
    protected override string Title => "Get project settings in YAML ...";
    protected override string Tag => "project yaml";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager is null) throw new ArgumentNullException(nameof(apiManager));
        var result = await apiManager.GetProjectYamlSettingsAsync(Slug, ct).ConfigureAwait(false);
        return result;
    }
}

#nullable restore
