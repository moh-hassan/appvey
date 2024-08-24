// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;
using Api.Model;
using AppVeyorCli;
using RestApi.Extensions;

[CliCommand(Name = "build-version", Description = "Get project build by version",
    Parent = typeof(AppveyorCommand.ProjectCommand))]
public class BuildByVersion : AppveyorCommandBase
{
    protected override string Title => "Get project build by version ...";
    protected override string Tag => "project build-version";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliArgument(Description = "Build Version", Required = true)]
    public string BuildVersion { get; set; }

    //todo : add support for downloading artifacts
    //[CliOption(Description = "Where to save the downloaded artifacts", Required = false)]
    //public DirectoryInfo Artifact { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        var response = await apiManager.GetProjectBuildByVersionAsync(Slug, BuildVersion, ct);
        Request = response.Request;
        return response;
    }

    protected override void DisplayResponseResult(ResponseResult result)
    {
        if (!result.IsSuccess) return;
        var buildId = result.ResponseString.ToObject<BuildInfo>().Build.BuildId;
        var pageUrl = ApiEndpoints.GetBuildPageUrl(Account, Slug, buildId);
        WriteLine($"Browse Build page> {pageUrl}");

        var summary = result.ProjectBuildByVersion();
        WriteLine("Response summary:");
        WriteLine(summary);
    }
}

#nullable restore
