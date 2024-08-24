// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Build;

#nullable disable
using Api;
using Api.Exceptions;

[CliCommand(Description = "Download build log", Name = "log",
    Parent = typeof(AppveyorCommand.BuildCommand.DownloadCommand))]
public class DownloadLog : AppveyorCommandBase
{
    protected override string Title => "Download build log ...";
    protected override string Tag => "build download log";

    [CliOption(Description = "Slug/Project name.", Required = true)]
    public string Slug { get; set; }

    [CliOption(Description = "Build JobId.", Required = false)]
    public string JobId { get; set; }

    [CliOption(Description = "Build version. Both job-id and build-version are mutually exclusive.", Required = false)]
    public string Version { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));
        ValidateOptions(Version, JobId);
        if (!string.IsNullOrEmpty(Version))
        {
            var buildInfo = await apiManager.GetBuildInfoAsync(Slug, Version, ct).ConfigureAwait(false);
            JobId = buildInfo?.Build.Jobs[0].JobId;
            if (string.IsNullOrEmpty(JobId))
            {
              WriteWarning("No job found for the specified build-version");
              return ResponseResult.Default();
            }
        }

        if (string.IsNullOrEmpty(JobId))
        {
            throw new AppveyorException("job-id is required");
        }

        var result = await apiManager.DownloadBuildLogAsync(JobId, ct).ConfigureAwait(false);
        return result;
    }
}
#nullable restore
