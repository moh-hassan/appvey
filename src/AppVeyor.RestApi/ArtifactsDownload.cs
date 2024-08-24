// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Model;
using RestApi.Extensions;

public class ArtifactsDownload : IDisposable
{
    private ApiManager ApiManager { get; }
    private HttpClient Client { get; }
    private string Account { get; }

    public ArtifactsDownload(ApiManager apiManager)
    {
        ApiManager = apiManager;
        Account = apiManager.Account;
        var apiClient = apiManager.ApiClient;
        Client = apiClient.GetClient();
    }

    public async Task<int> GetAppVeyorArtifactsByJobId(
        string project,
        string? branch = null,
        bool list = false,
        string downloadDirectory = ".",
        string? buildJobId = null,
        string? jobName = null,
        string filter = "",
    bool flat = false,
        CancellationToken ct = default)
    {
        var projectUri = $"/api/projects/{Account}/{project}";
        if (!string.IsNullOrEmpty(branch))
        {
            projectUri += $"/branch/{branch}";
        }

        var jsonBuild = await Client.GetStringAsync(projectUri, ct);

        var buildInfo = jsonBuild.ToObject<BuildInfo>();
        if (buildInfo?.Build.Jobs == null)
        {
            WriteWarning("No jobs found for this project or the project and/or account name was incorrectly specified");
            return 0;
        }

        string? jobId;
        if (string.IsNullOrEmpty(buildJobId)) //last build
        {
            if (buildInfo.Build.Jobs.Count > 1 && string.IsNullOrEmpty(jobName))
            {
                WriteWarning(
                    "Multiple Jobs found for the latest build. Please specify the -JobName parameter to select which job you want the artifacts for");
                return 0;
            }

            if (!string.IsNullOrEmpty(jobName))
            {
                WriteInfo($"Searching for job {jobName} within the latest build");
                jobId = buildInfo.Build.Jobs.FirstOrDefault(job => job.Name == jobName)?.JobId;
                if (string.IsNullOrEmpty(jobId))
                {
                    WriteWarning(
                        $"Unable to find a job named {jobName} within the specified build. Did you spell it correctly?");
                    return 0;
                }
            }
            else
            {
                jobId = buildInfo.Build.Jobs[0].JobId;
            }
        }
        else
        {
            jobId = buildJobId;
        }

        WriteInfo($"jobId: {jobId}");
        var artifactsUri = $"/api/buildjobs/{jobId}/artifacts";
        var artifacts = await GetArtifacts(artifactsUri, ct);

        if (artifacts == null || artifacts.Count == 0)
        {
            WriteWarning("No artifacts found.");
            return 0;
        }

        if (!list && artifacts.Count > 0)
            WriteInfo("Start downloading ...");
        var count = 0;
        if (list && artifacts.Count > 0)
            WriteInfo($"Listing aAvailable artifacts ({artifacts.Count}):");
        foreach (var artifact in artifacts)
        {
            var localArtifactPath = artifact.FileName;
            localArtifactPath = flat
                ? Path.GetFileName(localArtifactPath)
                : Path.Combine(downloadDirectory, localArtifactPath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            var artifactUrl = $"/api/buildjobs/{jobId}/artifacts/{artifact.FileName}";
            if (list)
            {
                Console.WriteLine(artifact.FileName);
                continue;
            }

            //check if the fileName matches the filter
            var canDownload = IsMatch(localArtifactPath, filter);
            // Console.WriteLine($"{localArtifactPath}| filter: {filter} |canDownload: {canDownload}");
            if (!canDownload) continue;

            //Ensure the directory exists
            var directoryPath = Path.GetDirectoryName(localArtifactPath);
            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var size = await Download(artifactUrl, localArtifactPath, ct);

            // Console.WriteLine($"Downloaded {artifactUrl} to {localArtifactPath} size = {size:N0} bytes");
            WriteColoredText($"[white]Downloaded {artifactUrl} to  [\\]  [green] {localArtifactPath} [\\] [yellow] size= {size:N0} [\\]");
            count++;
        }

        WriteInfo($"Total download files = {count}");
        return count;
    }

    public async Task<int> GetAppVeyorArtifactsByVersion(
        string project,
        string version,
        bool list = false,
        string downloadDirectory = ".",
        string filter = "",
        bool flat = false,
        CancellationToken ct = default)
    {
        var buildInfo = await ApiManager.GetBuildInfoAsync(project, version, ct);

        if (buildInfo == null)
        {
            WriteWarning("No jobs found for this project and version or it was incorrectly specified.");
            return 0;
        }

        var jobId = buildInfo.Build.Jobs[0].JobId;
        var count = await GetAppVeyorArtifactsByJobId(project, list: list, downloadDirectory: downloadDirectory, buildJobId: jobId, filter: filter, flat: flat, ct: ct);
        return count;
    }

    private bool IsMatch(string name, string pattern)
    {
        return string.IsNullOrEmpty(pattern) || Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase);
    }

    private async Task<List<Artifact>?> GetArtifacts(string artifactsUri, CancellationToken ct)
    {
        var artifactsResponse = await Client.GetAsync(artifactsUri, ct);
        artifactsResponse.EnsureSuccessStatusCode();
        var json = await artifactsResponse.Content.ReadAsStringAsync(ct);
        var artifacts = json.ToObject<List<Artifact>>();
        return artifacts;
    }

    private async Task<int> Download(
        string artifactUrl,
        string localArtifactPath,
        CancellationToken ct)
    {
        var artifactResponse = await Client.GetAsync(artifactUrl, ct);
        artifactResponse.EnsureSuccessStatusCode();
        var artifactData = await artifactResponse.Content.ReadAsByteArrayAsync(ct);
        var size = artifactData.Length;
        await File.WriteAllBytesAsync(localArtifactPath, artifactData, ct);
        return size;
    }

    private async Task<string> GetStringAsync(string url, CancellationToken ct)
    {
        var projectResponse = await Client.GetAsync(url, ct);
        projectResponse.EnsureSuccessStatusCode();
        var json = await projectResponse.Content.ReadAsStringAsync(ct);
        return json;
    }

    public void Dispose()
    {
        Client.Dispose();
        ApiManager.Dispose();
    }
}
