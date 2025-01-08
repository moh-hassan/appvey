// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;


[CliCommand(
    Description = "Update project settings from YAML file.\n" +
    "Example:\n" +
    "\tappvey project update yaml -a my-account -t my-token -s my-project setting.yaml",
    Name = "yaml",
    Aliases = ["yml"],
    Parent = typeof(AppveyorCommand.ProjectCommand.UpdateCommand))]
public class UpdateYamlSetting : AppveyorCommandBase
{
    protected override string Title => "Update project settings in YAML ...";
    protected override string Tag => "project update yaml";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliArgument(Description = "YAML file that contain project settings",
        Required = true,
        HelpName = "yaml-file",
        ValidationRules = CliValidationRules.ExistingFile)]
    public FileInfo YamlSetting { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager is null) throw new ArgumentNullException(nameof(apiManager));
        var result = await apiManager
            .UpdateProjectSettingsInYamlAsync(Slug, YamlSetting, WhatIf, ct);
        return result;
    }
}
#nullable restore

