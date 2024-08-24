// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;
using Api.Collection;

[CliCommand(Description = "Update project environment variables",
        Parent = typeof(AppveyorCommand.ProjectCommand.UpdateCommand), Name = "env")]
public class UpdateEnvironment : AppveyorCommandBase
{
    protected override string Title => "Update project environment variables ...";
    protected override string Tag => "project update env";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliArgument(Description = "environment var in the form name:value. If it's encrypted, it's in the form name:value:true", Required = false)]
    public List<string> Env { get; set; }

    [CliOption(Description = "File name that contain environment json string. Only Env arguments or Json option is allowed", Required = false, ValidationRules = CliValidationRules.ExistingFile)]
    public FileInfo Json { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager is null) throw new ArgumentNullException(nameof(apiManager));

        if (!IsValidOptions())
        {
            throw new ArgumentException("Invalid options, only Env arguments or Json option is allowed.");
        }

        if (Env is { Count: > 0 })
        {
            var environment = new EncryptedEnvironmentCollection(Env);
            var result = await apiManager
                .UpdateProjectEnvironmentVariablesAsync(Slug, environment, ct)
                .ConfigureAwait(false);
            return result;
        }

        if (Json != null)
        {
            var result = await apiManager
                .UpdateProjectEnvironmentVariablesAsync(Slug, Json, ct)
                .ConfigureAwait(false);
            return result;
        }

        return default;
    }

    internal bool IsValidOptions()
    {
        //check if Env and Json are mutually exclusive
        if (Env is { Count: > 0 } && Json != null)
            return false;
        //only one of the options should be provided
        if (Env is { Count: > 0 } || Json != null)
            return true;
        return false;
    }
}
#nullable restore
