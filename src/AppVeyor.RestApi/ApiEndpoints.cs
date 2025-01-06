// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

public static class ApiEndpoints
{
    public const string AppVeyorBaseApi = "https://ci.appveyor.com";
    public const string MockBaseApi = "http://localhost:5001/";

    //Manage the base url for the api by checking the environment variable DEVELOP
    public static string GetBaseUrl()
    {
        var develop = Environment.GetEnvironmentVariable("DEVELOP") ?? "0";
        if (develop == "1")
        {
            return MockBaseApi;
        }

        return AppVeyorBaseApi;
    }

    public static string ApiUrl(string accountName)
            => $"/api/account/{accountName}";

    public static string GetBuildPageUrl(string account, string projectSlug, object buildId)
        => $"{AppVeyorBaseApi}/project/{account}/{projectSlug}/builds/{buildId}";

    //---------------------------Projects API--------------------------------
    public static string GetProjectsUrl(string accountName)
        => $"/api/account/{accountName}/projects";

    public static string GetProjectLastBranchBuildUrl(
        string accountName,
        string projectSlug,
        string? buildBranch = null)
    {
        return string.IsNullOrEmpty(buildBranch)
            ? $"/api/projects/{accountName}/{projectSlug}"
            : $"/api/projects/{accountName}/{projectSlug}/branch/{buildBranch}";
    }

    public static string GetProjectBuildByVersionUrl(
        string accountName,
        string projectSlug,
        string buildVersion)
        => $"/api/projects/{accountName}/{projectSlug}/build/{buildVersion}";

    public static string GetProjectHistoryUrl(string accountName, string projectSlug)
        => $"/api/projects/{accountName}/{projectSlug}/history";

    public static string GetProjectDeploymentUrl(string accountName, string projectSlug)
        => $"/api/projects/{accountName}/{projectSlug}/deployments";

    public static string GetProjectSettingsUrl(string accountName, string projectSlug)
        => $"/api/projects/{accountName}/{projectSlug}/settings";

    public static string GetProjectYamlSettingUrl(string accountName, string projectSlug)
        => $"/api/projects/{accountName}/{projectSlug}/settings/yaml";

    public static string GetProjectEnvironmentUrl(string accountName, string projectSlug)
        => $"/api/projects/{accountName}/{projectSlug}/settings/environment-variables";

    public static string PostAddProjectUrl(string accountName)
       => $"{ApiUrl(accountName)}/projects";

    public static string PutUpdateProjectUrl(string accountName)
        => $"{ApiUrl(accountName)}/projects";

    public static string PutUpdateProjectSettingsInYamlUrl(string accountName, string projectSlug)
        => $"/api/projects/{accountName}/{projectSlug}/settings/yaml";

    public static string PutUpdateProjectEnvironmentVariablesUrl(string accountName, string projectSlug)
        => $"/api/projects/{accountName}/{projectSlug}/settings/environment-variables";

    public static string PutProjectBuildNumberUrl(string accountName, string projectSlug)
        => $"/api/projects/{accountName}/{projectSlug}/settings/build-number";

    public static string DeleteProjectBuildCacheUrl(string accountName, string projectSlug)
      => $"/api/projects/{accountName}/{projectSlug}/buildcache";

    public static string DeleteProjectUrl(string accountName, string projectSlug)
        => $"/api/projects/{accountName}/{projectSlug}";

    //---------------------------Builds API--------------------------------
    public static string PostMostRecentCommitUrl(string accountName)
        => $"/api/account/{accountName}/builds";

    public static string GetReRunBuildUrl(string accountName)
        => $"/api/account/{accountName}/builds";

    public static string PostStartBuildPrUrl(string accountName)
        => $"{ApiUrl(accountName)}/builds";

    public static string CancelBuildUrl(string accountName, string projectSlug, string buildVersion)
        => $"/api/builds/{accountName}/{projectSlug}/{buildVersion}";

    public static string GetDownloadBuildLogUrl(string jobId)
        => $"/api/buildjobs/{jobId}/log";


    public static string GetBuildUrl(string accountName, string buildId)
        => $"/api/account/{accountName}/builds/{buildId}";

    public static string GetRootArtifact(string jobId)
        => $"/api/buildjobs/{jobId}/artifacts";

    public static string GetArtifact(string jobId, string artifactName)
        => $"/api/buildjobs/{jobId}/artifacts/{artifactName}";

    //-----------------------------Environments API-------------------------
    public static string GetEnvironmentsUrl(string accountName)
        => $"/api/account/{accountName}/environments";

    public static string GetEnvironmentSettings(string deploymentEnvironmentId)
        => $"/api/environments/{deploymentEnvironmentId}/settings";

    public static string GetEnvironmentDeployments(string deploymentEnvironmentId)
    => $"/api/environments/{deploymentEnvironmentId}/deployments";

    //-----------------------------Teams api----------------------
    public static string GetUsersUrl(string accountName)
       => $"{ApiUrl(accountName)}/users";

    public static string GetUsersUrl(string accountName, string userId)
        => $"{ApiUrl(accountName)}/users/{userId}";

    public static string PostUsersUrl(string accountName) => GetUsersUrl(accountName);

    public static string PutUsersUrl(string accountName) => GetUsersUrl(accountName);

    public static string DeleteUsersUrl(string accountName, string userId) => GetUsersUrl(accountName, userId);

    public static string GetCollaboratorsUrl(string accountName)
        => $"{ApiUrl(accountName)}/collaborators";

    public static string GetCollaboratorUrl(string accountName, string userId)
        => $"{ApiUrl(accountName)}/collaborators/{userId}";

    public static string PostCollaboratorsUrl(string accountName) => GetCollaboratorsUrl(accountName);

    public static string PutCollaboratorsUrl(string accountName) => GetCollaboratorsUrl(accountName);

    public static string DelCollaboratorUrl(string accountName, string userId)
        => GetCollaboratorUrl(accountName, userId);

    public static string GetRolesUrl(string accountName)
        => $"{ApiUrl(accountName)}/roles";

    public static string GetRoleUrl(string accountName, string roleId)
        => $"{ApiUrl(accountName)}/roles/{roleId}";

    public static string PostRoleUrl(string accountName)
        => $"{ApiUrl(accountName)}/roles";

    public static string PutRoleUrl(string accountName) => PostRoleUrl(accountName);

    public static string DelRoleUrl(string accountName, string roleId)
        => $"{ApiUrl(accountName)}/roles/{roleId}";

    public static string GetBaseUrl(string develop)
        => develop == "1" ? MockBaseApi : AppVeyorBaseApi;
}
