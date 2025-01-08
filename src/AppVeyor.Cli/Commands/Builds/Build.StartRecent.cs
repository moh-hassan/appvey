// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

#nullable disable
using System.Net;
using Api;
using Api.Collection;
using RestApi.Extensions;

[CliCommand(Description = Strings.Build_start_recent,
    Parent = typeof(AppveyorCommand.BuildCommand.StartCommand))]
public class Recent : AppveyorCommandBase, IBrowse
{
    protected override string Title => "Start build of branch most recent commit ...";
    protected override string Tag => "build start recent";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliOption(Description = "Repository branch.")]
    public string Branch { get; set; } = "master";

    public bool Browse { get; set; }

    [CliOption(Description = "Allow to cancel build", Required = false)]
    public bool Cancel { get; set; }

    [CliArgument(Description = "The list of key/value pairs separated by space.\nEvery variable should be in the form:\n  <varName:value> \n  OR <varName=value>\n  OR use file with @ prefix like <@filename> which contain the Var/Value pairs",
        Required = false)]
    public List<string> EnvironmentVariables { get; set; } = [];
    private EnvironmentDictionary _envs =>
        new EnvironmentDictionary(EnvironmentVariables.ToArray());

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));

        var result = await apiManager
            .StartBuildMostRecentAsync(Slug, Branch, _envs, WhatIf, ct);
        if (WhatIf)
        {
            // result.ShowResult();
            return result;
        }

        if (result.StatusCode != HttpStatusCode.OK)
        {
            WriteLine("Failed to start the build");
        }
        else
        {
            WriteLine("Build started successfully");
            _ = PrintBuildReport(result, Browse, Slug);
        }

        return result;
    }

    private async Task AllowCancel(ApiManager apiManager, CancellationToken ct, string version)
    {
        if (Cancel && Confirm("Cancel Build."))
        {
            await apiManager.CancelBuildAsync(Slug, version, WhatIf, ct);
        }
    }

    protected override async Task PostCommandAsync(
        ApiManager apiManager,
        ResponseResult result,
        CancellationToken ct)
    {
        await base.PostCommandAsync(apiManager, result, ct);
        var build = result.ResponseString.ToObject<RestApi.Model.Build>();
        if (build == null) return;
        await AllowCancel(apiManager, ct, build.Version);
    }
}
#nullable restore

