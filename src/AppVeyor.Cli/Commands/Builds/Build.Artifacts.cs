// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Build;

using System.Text.RegularExpressions;
#nullable disable
using Api;
using Api.Exceptions;

[CliCommand(Description = "Download build artifacts of last build or by a version in the project",
    Parent = typeof(AppveyorCommand.BuildCommand.DownloadCommand), ShortFormAutoGenerate = true)]
public class Artifacts : AppveyorCommandBase
{
    protected override string Title => "Download build artifacts ...";
    protected override string Tag => "build download artifacts";
    
    [CliOption(Description = "Slug name", Required = true)]
    public string Slug { get; set; }

    [CliOption(Description = "Repository branch", Required = false)]
    public string Branch { get; set; }

    [CliOption(Description = "Build JobId", Required = false)]
    public string JobId { get; set; }

    [CliOption(Description = "Build version. Both job-id and version are mutually exclusive, either JobId or Version must be specified", Required = false)]
    public string Version { get; set; }

    [CliOption(Description = "Directory where to save artifacts, relative or absolute path",
        Required = false)]
    public DirectoryInfo Location { get; set; } = new(".");

    [CliOption(Description = "Show artifacts without download", Name = "--list", Required = false)]
    public bool ShowOnly { get; set; }

    [CliOption(Description = "Filter artifacts using wildcard symbols", Required = false,
        AllowMultipleArgumentsPerToken = true, Arity = CliArgumentArity.OneOrMore)]
    public string[] Filter { get; set; }

    [CliOption(Required = false, Description = "File to save output response.", Hidden = true)]
    public new FileInfo Save { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager,
        CancellationToken ct)
    {
        var downloader = new ArtifactsDownload(apiManager);
        if (apiManager == null) throw new ArgumentNullException(nameof(apiManager));
        ValidateOptions(Version, JobId);
        var filter = GetPattern();
        var folder = Location.FullName;
        if (!string.IsNullOrEmpty(Version))
        {
            await downloader.GetAppVeyorArtifactsByVersion(
                Slug,
                version: Version,
                list: ShowOnly,
                downloadDirectory: folder,
                filter: filter,
               ct: ct).ConfigureAwait(false);
            return ResponseResult.Default();
        }

        if (!string.IsNullOrEmpty(JobId))
        {
            await downloader.GetAppVeyorArtifactsByJobId(
                Slug,
                branch: Branch,
                list: ShowOnly,
                buildJobId: JobId,
                downloadDirectory: folder,
                filter: filter,
                ct: ct).ConfigureAwait(false);
            return ResponseResult.Default();
        }

        return ResponseResult.Default();
    }

    //private void ValidateOptions()
    //{
    //    if (string.IsNullOrEmpty(JobId) && string.IsNullOrEmpty(BuildVersion))
    //    {
    //        throw new AppveyorException("Either JobId or Version must be specified");
    //    }

    //    if (!string.IsNullOrEmpty(JobId) && !string.IsNullOrEmpty(BuildVersion))
    //    {
    //        throw new AppveyorException("JobId and Version are mutually exclusive");
    //    }
    //}

    private string GetPattern()
    {
        if (Filter == null || Filter.Length == 0) return string.Empty;
        var result = Filter.Select(ConvertWildcardToRegex);
        return string.Join("|", result);
    }

    private string ConvertWildcardToRegex(string pattern)
    {
        return Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".");
    }
}
#nullable restore


