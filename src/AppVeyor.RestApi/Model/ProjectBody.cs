namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
internal class ProjectBody
{
}

public class AddProject
{
    [JsonProperty("repositoryProvider")]
    public string RepositoryProvider { get; set; }

    [JsonProperty("repositoryName")]
    public string RepositoryName { get; set; }
}

//Update project environment variables
// 
public class UpdateProjectEnvironmentVariables

{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("value")]
    public EnvTypeValue Value { get; set; }
}

public class EnvTypeValue
{
    [JsonProperty("isEncrypted")]
    public bool IsEncrypted { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}

//---------------

//Update project build number

public class UpdateProjectBuildNumberBody
{
    [JsonProperty("nextBuildNumber")]
    public int NextBuildNumber { get; set; }
}

public class StartBuildBranchCommit
{
    [JsonProperty("accountName")]
    public string AccountName { get; set; }

    [JsonProperty("projectSlug")]
    public string ProjectSlug { get; set; }

    [JsonProperty("branch")]
    public string Branch { get; set; }

    [JsonProperty("commitId")]
    public string CommitId { get; set; }
}

public class ReRunBuildBody
{
    [JsonProperty("buildId")]
    public string BuildId { get; set; }

    //Set reRunIncomplete set to False (default value) for full buildre-run.
    //Set it set to True to rerun only failed or cancelled jobs in multijob build.

    [JsonProperty("reRunIncomplete")]
    public string ReRunIncomplete { get; set; }

    public ReRunBuildBody(string buildId, string reRunIncomplete)
    {
        BuildId = buildId;
        ReRunIncomplete = reRunIncomplete;
    }
}

public class StartBuildBranchMostRecentCommitBody
{
    [JsonProperty("accountName")]
    public string AccountName { get; set; }

    [JsonProperty("projectSlug")]
    public string ProjectSlug { get; set; }

    [JsonProperty("branch")]
    public string Branch { get; set; }

    [JsonProperty("environmentVariables")]
    public Dictionary<string, string> EnvironmentVariables { get; set; }

    public StartBuildBranchMostRecentCommitBody(
        string accountName,
        string projectSlug,
        string branch,
        Dictionary<string, string> environmentVariables)
    {
        AccountName = accountName;
        ProjectSlug = projectSlug;
        Branch = branch;
        EnvironmentVariables = environmentVariables;
    }
}

public class StartBuildOPullRequestBody
{
    [JsonProperty("accountName")]
    public string AccountName { get; set; }

    [JsonProperty("projectSlug")]
    public string ProjectSlug { get; set; }

    [JsonProperty("pullRequestId")]
    public int PullRequestId { get; set; }
}





#nullable restore
