// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Build;

using Api;

#nullable disable

[CliCommand(Description = "Start build of Pull Request",
    Parent = typeof(AppveyorCommand.BuildCommand.StartCommand))]
public class Pr : AppveyorCommandBase
{
    protected override string Title => "Start build of Pull Request ...";
    protected override string Tag => "build start pr";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliArgument(Description = "Start Pull Request", Required = true)]
    public string PrId { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));

        var result = await apiManager.StartBuildPrAsync(Slug, PrId, ct).ConfigureAwait(false);
        return result;
    }
}
#nullable restore

