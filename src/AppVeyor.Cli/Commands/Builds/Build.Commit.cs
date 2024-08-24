// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

using System.Net;
using Api;

#nullable disable
[CliCommand(Description = "Start build of specific branch commit", Parent = typeof(AppveyorCommand.BuildCommand.StartCommand))]
public class Commit : AppveyorCommandBase
{
    protected override string Title => "Start build of specific branch commit ...";
    protected override string Tag => "build start commit";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliOption(Description = "Repository branch", Required = true)]
    public string Branch { get; set; }

    [CliOption(Description = "Browse to the build page", Required = false)]
    public bool Browse { get; set; }

    [CliArgument(Description = "Commit Id of specific branch", Required = true)]
    public string CommitId { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));

        _ = IsValidOptions();
        var result = await apiManager.StartBuildCommitAsync(Slug, Branch, CommitId, ct);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            WriteLine("Failed to start the build");
        }
        else
        {
            WriteLine("Build started successfully");
            PrintBuildReport(result, Browse, Slug);
        }

        return result;
    }

    private bool IsValidOptions()
    {
        if (string.IsNullOrEmpty(Slug))
        {
            throw new ArgumentException("slug option is null");
        }

        if (string.IsNullOrEmpty(Branch))
        {
            throw new ArgumentException("branch option is null");
        }

        return true;
    }
}
#nullable restore




