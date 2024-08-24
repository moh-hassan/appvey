// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Build;

using Api;

[CliCommand(Description = "Delete build", Name = "delete",
    Parent = typeof(AppveyorCommand.BuildCommand))]
public class BuildDelete : AppveyorCommandBase
{
    protected override string Title => "Delete build ...";
    protected override string Tag => "build delete";

    [CliArgument(Arity = CliArgumentArity.OneOrMore, Required = true)]
    public string[] BuildId { get; set; } = [];

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));

        var result = await apiManager.DeleteBuildsAsync(BuildId, ct);
        return result;
    }
}
