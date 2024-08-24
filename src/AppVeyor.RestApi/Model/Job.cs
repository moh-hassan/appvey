#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class Job
{
    [JsonProperty("jobId")]
    public string JobId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("osType")]
    public string OsType { get; set; }

    [JsonProperty("allowFailure")]
    public bool AllowFailure { get; set; }

    [JsonProperty("messagesCount")]
    public int MessagesCount { get; set; }

    [JsonProperty("compilationMessagesCount")]
    public int CompilationMessagesCount { get; set; }

    [JsonProperty("compilationErrorsCount")]
    public int CompilationErrorsCount { get; set; }

    [JsonProperty("compilationWarningsCount")]
    public int CompilationWarningsCount { get; set; }

    [JsonProperty("testsCount")]
    public int TestsCount { get; set; }

    [JsonProperty("passedTestsCount")]
    public int PassedTestsCount { get; set; }

    [JsonProperty("failedTestsCount")]
    public int FailedTestsCount { get; set; }

    [JsonProperty("artifactsCount")]
    public int ArtifactsCount { get; set; }

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
