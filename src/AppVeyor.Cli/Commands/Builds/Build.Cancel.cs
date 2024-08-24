// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Build;

#nullable disable
using Api;

[CliCommand(Description = "Cancel build", Parent = typeof(AppveyorCommand.BuildCommand))]
public class Cancel : AppveyorCommandBase
{
    protected override string Title => "Cancel build ...";
    protected override string Tag => "build cancel";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliArgument(Description = "Build Version", Required = true)]
    public string BuildVersion { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));
        var result = await apiManager.CancelBuildAsync(Slug, BuildVersion, ct);
        return result;
    }
}

#nullable restore
