// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using System.Collections.Generic;
using System.Threading.Tasks;
using Collection;
using Extensions;
using Model;
using RestApi.Extensions;

public partial class ApiManager : IDisposable
{
    internal ApiClient ApiClient { get; }
    internal string Account { get; }

    public ApiManager(HttpConnection httpConnection)
    {
        ApiClient = ApiClient.Create(httpConnection);
        Account = httpConnection.AccountCredential.UserName;        
    }

    #region Project API

    public async Task<ResponseResult> GetProjectsAsync(bool whatif = false, CancellationToken ct = default)
    {
        var apiUrl = GetProjectsUrl(Account);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl);
        var response = await ApiClient.ExecuteApiAsync(apiUrl, ct:ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectLastBranchBuildAsync(
        string slug,
        string? branch = null,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectLastBranchBuildUrl(Account, slug, branch);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectBuildByVersionAsync(
        string slug,
        string buildVersion,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectBuildByVersionUrl(Account, slug, buildVersion);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    internal async Task<BuildInfo?> GetBuildInfoAsync(
        string slug,
        string buildVersion,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var response = await GetProjectBuildByVersionAsync(slug, buildVersion, whatif, ct);         
        return !response.IsSuccess ? null : response.ResponseString.ToObject<BuildInfo>();
    }

    public async Task<ResponseResult> GetProjectHistoryAsync(
        string slug,
        string? branch,
        int recordsNumber = 20,
        string? startBuildId = null,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectHistoryUrl(Account, slug);
        apiUrl = apiUrl
            .AddQueryString("recordsNumber", recordsNumber.ToString())
            .AddQueryString("branch", branch)
            .AddQueryString("startBuildId", startBuildId);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectDeploymentsAsync(
        string slug,
        string? startDeploymentId = null,
        int recordsNumber = 20,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectDeploymentUrl(Account, slug);
        apiUrl = apiUrl
              .AddQueryString("recordsNumber", recordsNumber.ToString())
              .AddQueryString("startDeploymentId", startDeploymentId);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectSettingsAsync(string slug,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectSettingsUrl(Account, slug);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectYamlSettingsAsync(
        string slug,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectYamlSettingUrl(Account, slug);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> GetProjectEnvironmentAsync(
        string slug,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = GetProjectEnvironmentUrl(Account, slug);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> AddProjectAsync(string repositoryProvider,
        string repositoryName,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = PostAddProjectUrl(Account);
        var body = RestBody.PostProjectBody(repositoryProvider, repositoryName);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "post", body);
        return await ApiClient.PostApiAsync(apiUrl, body, ct);
    }

    public async Task<ResponseResult> UpdateProjectEnvironmentVariablesAsync(
        string slug,
        FileInfo json,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = PutUpdateProjectEnvironmentVariablesUrl(Account, slug);
        var jsonContent = json.ReadFile();
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "put", jsonContent);
        return await ApiClient.PutApiAsync(apiUrl, jsonContent, ct);
    }

    public async Task<ResponseResult> UpdateProjectEnvironmentVariablesAsync(
        string slug,
        EncryptedEnvironmentCollection envs,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var json = envs.ToJson(true);
        var apiUrl = PutUpdateProjectEnvironmentVariablesUrl(Account, slug);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "put", json);
        return await ApiClient.PutApiAsync(apiUrl, json, ct);
    }

    public async Task<ResponseResult> UpdateProjectAsync(
        FileInfo jsonFile,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = PutUpdateProjectUrl(Account);
        var json = jsonFile.ReadFile();
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "put", json);
        return await ApiClient.PutApiAsync(apiUrl, json, ct);
    }

    public async Task<ResponseResult> UpdateProjectSettingsInYamlAsync(
        string slug,
        FileInfo yaml,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = PutUpdateProjectSettingsInYamlUrl(Account, slug);
        var yamlContent = yaml.ReadFile();
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "put", yamlContent);
        return await ApiClient.PutApiAsync(apiUrl, yamlContent, ct);
    }

    public async Task<ResponseResult> UpdateProjectBuildNumberAsync(
        string projectSlug,
        int nextBuildNumber,
        bool whatif = false,
        CancellationToken ct = default)
    {
        WriteLine($"Update Project Build Number for project: {projectSlug} to {nextBuildNumber}");
        var apiUrl = PutProjectBuildNumberUrl(Account, projectSlug);
        var body = new { nextBuildNumber }.ToJson();
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "put", body);
        return await ApiClient.PutApiAsync(apiUrl, body, ct);
    }

    public async Task<ResponseResult> DeleteProjectBuildCacheAsync(
        string projectSlug,
        bool whatif = false,
        CancellationToken ct = default)
    {
        WriteLine($"Delete Project Build Cache for project: {projectSlug}");
        var apiUrl = DeleteProjectBuildCacheUrl(Account, projectSlug);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "delete");
        return await ApiClient.DeleteApiAsync(apiUrl, ct);
    }

    public async Task<ResponseResult> DeleteProjectAsync(string projectSlug,
        bool whatif = false,
        CancellationToken ct=default)
    {
        var apiUrl = DeleteProjectUrl(Account, projectSlug);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "delete");
        return await ApiClient.DeleteApiAsync(apiUrl, ct);
    }

    #endregion

    #region Build Api

    public async Task<ResponseResult> StartBuildMostRecentAsync(
        string slug,
        string branch,
        EnvironmentDictionary? envs = null,
        bool whatif = false,
        CancellationToken ct = default)
    {
        envs ??= [];
        var apiUrl = PostMostRecentCommitUrl(Account);
        var body = RestBody.BuildMostRecentBody(Account, slug, branch, envs);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "post", body);

        var response = await ApiClient.PostApiAsync(apiUrl, body, ct);
        return response;
    }

    public async Task<ResponseResult> StartBuildCommitAsync(
        string slug,
        string branch,
        string commitId,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = PostMostRecentCommitUrl(Account);
        var body = RestBody.BuildCommitBody(Account, slug, branch, commitId);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "post", body);
        var response = await ApiClient.PostApiAsync(apiUrl, body, ct);
        return response;
    }

    public async Task<ResponseResult> ReRunBuildCommitAsync(
        string buildId,
        bool reRunIncomplete = false,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = PostMostRecentCommitUrl(Account);
        var body = RestBody.ReRunBuildBody(buildId, reRunIncomplete);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "post", body);
        var response = await ApiClient.PutApiAsync(apiUrl, body, ct);
        return response;
    }

    public async Task<ResponseResult> StartBuildPrAsync(
        string projectSlug,
        string pullRequestId,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = PostStartBuildPrUrl(Account);
        var body = RestBody.PostStartBuildPrBody(Account, projectSlug, pullRequestId);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "post", body);
        var response = await ApiClient.PostApiAsync(apiUrl, body, ct);
        return response;
    }

    public async Task<ResponseResult> CancelBuildAsync(
        string projectSlug,
        string buildVersion,
        bool whatif = false,
        CancellationToken ct = default)
    {
        var apiUrl = CancelBuildUrl(Account, projectSlug, buildVersion);
        if (whatif)
            return ResponseResult.WhatIfRequest(apiUrl, "delete");
        WriteLine($"Build version: {buildVersion} has been cancelled by a user.");
        var response = await ApiClient.DeleteApiAsync(apiUrl, ct);
        return response;
    }

    public async Task<ResponseResult> DeleteBuildsAsync(
        IList<string> buildIds,
        bool whatif = false,
        CancellationToken ct = default)
    {
        ResponseResult? response = null;
        if (whatif) {
            foreach (var buildId in buildIds)
            {
                var apiUrl = GetBuildUrl(Account, buildId);
                response = ResponseResult.WhatIfRequest(apiUrl, "delete");
                WriteLine($"WhatIf: Deleted build {buildId} with response: {response.StatusCode}");
            }
            return response!;
        }
        if (buildIds.Count == 0)
            return response!;
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
        bool whatif = false,
        CancellationToken ct = default)
    {
        jobId = jobId ?? throw new ArgumentNullException(nameof(jobId));
        if (whatif)
            return ResponseResult.WhatIfRequest(ApiEndpoints.GetDownloadBuildLogUrl(jobId));
        var apiUrl = GetDownloadBuildLogUrl(jobId);
        var response = await ApiClient.GetApiAsync(apiUrl, ct);
        return response;
    }

    #endregion

    public async Task<ResponseResult> RunHttpApiAsync(
        string url,
        HttpRequest method = HttpRequest.Get,
        string? json = null,
        bool whatif = false,
        CancellationToken ct = default)
    {
        return method switch
        {
            HttpRequest.Get => await ApiClient.GetApiAsync(url, ct),
            HttpRequest.Post => await ApiClient.PostApiAsync(url, json!, ct),
            HttpRequest.Put => await ApiClient.PutApiAsync(url, json!, ct),
            HttpRequest.Delete => await ApiClient.DeleteApiAsync(url, ct),
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

