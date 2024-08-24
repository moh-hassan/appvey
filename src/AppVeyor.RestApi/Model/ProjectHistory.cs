namespace AppVeyor.RestApi.Model;
#nullable disable
using Newtonsoft.Json;
using System.Collections.Generic;

public class ProjectHistory
{
    [JsonProperty("project")]
    public Project Project { get; set; }

    [JsonProperty("builds")]
    public List<Build> Builds { get; set; }
}

#nullable restore
