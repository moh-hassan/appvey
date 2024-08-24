﻿// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class Settings
{
    [JsonProperty("projectId")]
    public int ProjectId { get; set; }

    [JsonProperty("accountId")]
    public int AccountId { get; set; }

    [JsonProperty("accountName")]
    public string AccountName { get; set; }

    [JsonProperty("builds")]
    public List<object> Builds { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("slug")]
    public string Slug { get; set; }

    [JsonProperty("versionFormat")]
    public string VersionFormat { get; set; }

    [JsonProperty("nextBuildNumber")]
    public int NextBuildNumber { get; set; }

    [JsonProperty("repositoryType")]
    public string RepositoryType { get; set; }

    [JsonProperty("repositoryScm")]
    public string RepositoryScm { get; set; }

    [JsonProperty("repositoryName")]
    public string RepositoryName { get; set; }

    [JsonProperty("repositoryBranch")]
    public string RepositoryBranch { get; set; }

    [JsonProperty("webhookId")]
    public string WebhookId { get; set; }

    [JsonProperty("webhookUrl")]
    public string WebhookUrl { get; set; }

    [JsonProperty("statusBadgeId")]
    public string StatusBadgeId { get; set; }

    [JsonProperty("isPrivate")]
    public bool IsPrivate { get; set; }

    [JsonProperty("isGitHubApp")]
    public bool IsGitHubApp { get; set; }

    [JsonProperty("ignoreAppveyorYml")]
    public bool IgnoreAppveyorYml { get; set; }

    [JsonProperty("skipBranchesWithoutAppveyorYml")]
    public bool SkipBranchesWithoutAppveyorYml { get; set; }

    [JsonProperty("enableSecureVariablesInPullRequests")]
    public bool EnableSecureVariablesInPullRequests { get; set; }

    [JsonProperty("enableSecureVariablesInPullRequestsFromSameRepo")]
    public bool EnableSecureVariablesInPullRequestsFromSameRepo { get; set; }

    [JsonProperty("enableDeploymentInPullRequests")]
    public bool EnableDeploymentInPullRequests { get; set; }

    [JsonProperty("saveBuildCacheInPullRequests")]
    public bool SaveBuildCacheInPullRequests { get; set; }

    [JsonProperty("rollingBuilds")]
    public bool RollingBuilds { get; set; }

    [JsonProperty("rollingBuildsDoNotCancelRunningBuilds")]
    public bool RollingBuildsDoNotCancelRunningBuilds { get; set; }

    [JsonProperty("rollingBuildsOnlyForPullRequests")]
    public bool RollingBuildsOnlyForPullRequests { get; set; }

    [JsonProperty("alwaysBuildClosedPullRequests")]
    public bool AlwaysBuildClosedPullRequests { get; set; }

    [JsonProperty("configuration")]
    public Configuration Configuration { get; set; }

    [JsonProperty("tags")]
    public string Tags { get; set; }

    [JsonProperty("nuGetFeed")]
    public NuGetFeed NuGetFeed { get; set; }

    [JsonProperty("securityDescriptor")]
    public SecurityDescriptor SecurityDescriptor { get; set; }

    [JsonProperty("disablePushWebhooks")]
    public bool DisablePushWebhooks { get; set; }

    [JsonProperty("disablePullRequestWebhooks")]
    public bool DisablePullRequestWebhooks { get; set; }

    [JsonProperty("created")]
    public DateTime Created { get; set; }

    [JsonProperty("updated")]
    public DateTime Updated { get; set; }
}
