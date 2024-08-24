namespace AppVeyor.RestApi.Model;
#nullable disable

using Newtonsoft.Json;

public class EncryptedEnvironmentVar
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("value")]
    public EnvironmentValue Value { get; set; }

    public EncryptedEnvironmentVar()
    {

    }

    public EncryptedEnvironmentVar(string name, object value, bool isEncrypted)
    {
        Name = name;
        Value = new EnvironmentValue(value, isEncrypted);
    }
}

#nullable restore
