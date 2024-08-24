namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

#nullable disable

public class AddRole

{
    [JsonProperty("name")]
    public string Name { get; set; }
}


#nullable restore
