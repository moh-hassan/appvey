namespace AppVeyor.RestApi.Model;

#nullable disable
using Newtonsoft.Json;

public class EnvironmentValue
{
    [JsonProperty("isEncrypted")] public bool IsEncrypted { get; set; }

    [JsonProperty("value")] public object Value { get; set; }

    public EnvironmentValue()
    {
    }

    public EnvironmentValue(object value, bool isEncrypted)
    {
        Value = value;
        IsEncrypted = isEncrypted;
    }
}


#nullable restore
