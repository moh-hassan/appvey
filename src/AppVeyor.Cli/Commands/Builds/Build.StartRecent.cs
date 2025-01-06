// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

#nullable disable
using System.Net;
using Api;
using Api.Collection;
using RestApi.Extensions;

[CliCommand(Description = "Start build of branch most recent commit.\nExample 1:\n appvey build start recent -s myproject --browse\nExample 2 using environment variables with = separator:\nappvey build start recent -s myproject --browse  var1=value1 var2=value2\nExample 3 using file:\nappvey build start recent -s myproject --browse @myenv.txt",
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

    [CliArgument(Description = "Environment variables", Required = false)]
    public List<string> EnvironmentVariables { get; set; } = [];

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));

        var envs = new EnvironmentDictionary(EnvironmentVariables.ToArray());
        var result = await apiManager.StartBuildMostRecentAsync(Slug, Branch, envs, ct);

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

