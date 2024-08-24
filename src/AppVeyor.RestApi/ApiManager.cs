// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using System.Collections.Generic;
using System.Threading.Tasks;
using Collection;
using Extensions;
using Model;
using RestApi.Extensions;

public class ApiManager : IDisposable
{
    internal ApiClient ApiClient { get; }
    internal string Account { get; }

    public ApiManager(HttpConnection httpConnection)
    {
        ApiClient = ApiClient.Create(httpConnection);
        Account = httpConnection.Account;
    }

    #region Project API

    public async Task<ResponseResult> GetProjectsAsync(CancellationToken ct = default)
    {
        var apiUrl = GetProjectsUrl(Account);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectLastBranchBuildAsync(
        string slug,
        string? branch = null,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectLastBranchBuildUrl(Account, slug, branch);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectBuildByVersionAsync(
        string slug,
        string buildVersion,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectBuildByVersionUrl(Account, slug, buildVersion);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    internal async Task<BuildInfo?> GetBuildInfoAsync(
        string slug,
        string buildVersion,
        CancellationToken ct = default)
    {
        var response = await GetProjectBuildByVersionAsync(slug, buildVersion, ct);
        return !response.IsSuccess ? null : response.ResponseString.ToObject<BuildInfo>();
    }

    public async Task<ResponseResult> GetProjectHistoryAsync(
        string slug,
        string? branch,
        int recordsNumber = 20,
        string? startBuildId = null,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectHistoryUrl(Account, slug);
        apiUrl = apiUrl
            .AddQueryString("recordsNumber", recordsNumber.ToString())
            .AddQueryString("branch", branch)
            .AddQueryString("startBuildId", startBuildId);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectDeploymentsAsync(
        string slug,
        string? startDeploymentId = null,
        int recordsNumber = 20,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectDeploymentUrl(Account, slug);
        apiUrl = apiUrl
              .AddQueryString("recordsNumber", recordsNumber.ToString())
              .AddQueryString("startDeploymentId", startDeploymentId);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectSettingsAsync(string slug, CancellationToken ct = default)
    {
        var apiUrl = GetProjectSettingUrl(Account, slug);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectYamlSettingsAsync(
        string slug,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectYamlSettingUrl(Account, slug);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectEnvironmentAsync(
        string slug,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectEnvironmentUrl(Account, slug);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> AddProjectAsync(string repositoryProvider, string repositoryName,
        CancellationToken ct = default)
    {
        var apiUrl = PostAddProjectUrl(Account);
        var body = RestBody.PostProjectBody(repositoryProvider, repositoryName);
        return await ApiClient.PostApiAsync(apiUrl, body, ct);
    }

    public async Task<ResponseResult> UpdateProjectEnvironmentVariablesAsync(
        string slug,
        FileInfo json,
        CancellationToken ct = default)
    {
        var apiUrl = PutUpdateProjectEnvironmentVariablesUrl(Account, slug);
        var jsonContent = json.ReadFile();
        return await ApiClient.PutApiAsync(apiUrl, jsonContent, ct);
    }

    public async Task<ResponseResult> UpdateProjectEnvironmentVariablesAsync(
        string slug,
        EncryptedEnvironmentCollection envs,
        CancellationToken ct = default)
    {
        var json = envs.ToJson(true);
        var apiUrl = PutUpdateProjectEnvironmentVariablesUrl(Account, slug);
        return await ApiClient.PutApiAsync(apiUrl, json, ct);
    }

    public async Task<ResponseResult> UpdateProjectAsync(
        FileInfo jsonFile,
        CancellationToken ct = default)
    {
        var apiUrl = PutUpdateProjectUrl(Account);
        var json = jsonFile.ReadFile();
        return await ApiClient.PutApiAsync(apiUrl, json, ct);
    }

    public async Task<ResponseResult> UpdateProjectSettingsInYamlAsync(
        string slug,
        FileInfo yaml,
        CancellationToken ct = default)
    {
        var apiUrl = PutUpdateProjectSettingsInYamlUrl(Account, slug);
        var yamlContent = yaml.ReadFile();
        return await ApiClient.PutApiAsync(apiUrl, yamlContent, ct);
    }

    public async Task<ResponseResult> UpdateProjectBuildNumberAsync(
        string projectSlug,
        int nextBuildNumber,
        CancellationToken ct = default)
    {
        WriteLine($"Update Project Build Number for project: {projectSlug} to {nextBuildNumber}");
        var apiUrl = PutProjectBuildNumberUrl(Account, projectSlug);
        var body = new { nextBuildNumber }.ToJson();
        return await ApiClient.PutApiAsync(apiUrl, body, ct);
    }

    public async Task<ResponseResult> DeleteProjectBuildCacheAsync(
        string projectSlug,
        CancellationToken ct = default)
    {
        WriteLine($"Delete Project Build Cache for project: {projectSlug}");
        var apiUrl = DeleteProjectBuildCacheUrl(Account, projectSlug);
        return await ApiClient.DeleteApiAsync(apiUrl, ct);
    }

    public async Task<ResponseResult> DeleteProjectAsync(string projectSlug, CancellationToken ct)
    {
        var apiUrl = DeleteProjectUrl(Account, projectSlug);
        return await ApiClient.DeleteApiAsync(apiUrl, ct);
    }

    #endregion

    #region Build Api

    public async Task<ResponseResult> StartBuildMostRecentAsync(
        string slug,
        string branch,
        EnvironmentDictionary? envs = null,
        CancellationToken ct = default)
    {
        envs ??= [];
        var apiUrl = PostMostRecentCommitUrl(Account);
        var body = RestBody.BuildMostRecentBody(Account, slug, branch, envs);
        var response = await ApiClient.PostApiAsync(apiUrl, body, ct);
        return response;
    }

    public async Task<ResponseResult> StartBuildCommitAsync(
        string slug,
        string branch,
        string commitId,
        CancellationToken ct = default)
    {
        var apiUrl = PostMostRecentCommitUrl(Account);
        var body = RestBody.BuildCommitBody(Account, slug, branch, commitId);
        var response = await ApiClient.PostApiAsync(apiUrl, body, ct);
        return response;
    }

    public async Task<ResponseResult> ReRunBuildCommitAsync(
        string buildId,
        bool reRunIncomplete = false,
        CancellationToken ct = default)
    {
        var apiUrl = PostMostRecentCommitUrl(Account);
        var body = RestBody.ReRunBuildBody(buildId, reRunIncomplete);
        var response = await ApiClient.PutApiAsync(apiUrl, body, ct);
        return response;
    }

    public async Task<ResponseResult> StartBuildPrAsync(
        string projectSlug,
        string pullRequestId,
        CancellationToken ct = default)
    {
        var apiUrl = PostStartBuildPrUrl(Account);
        var body = RestBody.PostStartBuildPrBody(Account, projectSlug, pullRequestId);
        var response = await ApiClient.PostApiAsync(apiUrl, body, ct);
        return response;
    }

    public async Task<ResponseResult> CancelBuildAsync(
        string projectSlug,
        string buildVersion,
        CancellationToken ct = default)
    {
        var apiUrl = CancelBuildUrl(Account, projectSlug, buildVersion);
        WriteLine($"Build version: {buildVersion} has been cancelled by a user.");
        var response = await ApiClient.DeleteApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> DeleteBuildsAsync(
        IList<string> buildIds,
        CancellationToken ct = default)
    {
        ResponseResult? response = null;
        foreach (var buildId in buildIds)
        {
            var apiUrl = GetBuildUrl(Account, buildId);
            response = await ApiClient.DeleteApiAsync(apiUrl, ct);
            WriteLine($"Deleted build {buildId} with response: {response.StatusCode}");
        }

        return response!; //last response
    }

    public async Task<ResponseResult> DownloadBuildLogAsync(
        string jobId,
        CancellationToken ct = default)
    {
        jobId = jobId ?? throw new ArgumentNullException(nameof(jobId));
        var apiUrl = GetDownloadBuildLogUrl(jobId);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    #endregion

    public async Task<ResponseResult> RunHttpApiAsync(
        string url,
        string method = "get",
        string? json = null,
        CancellationToken ct = default)
    {
        return method.ToLower() switch
        {
            "get" => await ApiClient.GetApiAsync(url, ct),
            "post" => await ApiClient.PostApiAsync(url, json!, ct),
            "put" => await ApiClient.PutApiAsync(url, json!, ct),
            "delete" => await ApiClient.DeleteApiAsync(url, ct),
            _ => throw new ArgumentException("Invalid http method")
        };
    }

    private void Dispose(bool disposing)
    {
        if (!disposing) return;
        ApiClient.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ApiManager()
    {
        Dispose(false);
    }
}

