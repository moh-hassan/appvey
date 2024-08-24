namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

//buildby version
#nullable disable
public class ProjectBuildInfo
{
    [JsonProperty("project")]
    public Project Project { get; set; }

    [JsonProperty("build")]
    public Build Build { get; set; }
}
#nullable restore
