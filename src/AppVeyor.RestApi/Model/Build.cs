
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

#nullable disable
public class Build
{
    [JsonProperty("buildId")]
    public int BuildId { get; set; }

    [JsonProperty("projectId")]
    public int ProjectId { get; set; }

    [JsonProperty("jobs")]
    public List<Job> Jobs { get; set; }

    [JsonProperty("buildNumber")]
    public int BuildNumber { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("branch")]
    public string Branch { get; set; }

    [JsonProperty("isTag")]
    public bool IsTag { get; set; }

    [JsonProperty("commitId")]
    public string CommitId { get; set; }

    [JsonProperty("authorName")]
    public string AuthorName { get; set; }

    [JsonProperty("authorUsername")]
    public string AuthorUsername { get; set; }

    [JsonProperty("committerName")]
    public string CommitterName { get; set; }

    [JsonProperty("committerUsername")]
    public string CommitterUsername { get; set; }

    [JsonProperty("committed")]
    public DateTime Committed { get; set; }

    [JsonProperty("messages")]
    public List<object> Messages { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("started")]
    public DateTime Started { get; set; }

    [JsonProperty("finished")]
    public DateTime Finished { get; set; }

    [JsonProperty("created")]
    public DateTime Created { get; set; }

    [JsonProperty("updated")]
    public DateTime Updated { get; set; }
}
#nullable restore
