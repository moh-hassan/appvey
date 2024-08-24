namespace AppVeyor.RestApi.Model;

#nullable disable
using Newtonsoft.Json;
using System.Collections.Generic;

public class SecurityDescriptor
{
    [JsonProperty("accessRightDefinitions")]
    public List<AccessRightDefinition> AccessRightDefinitions { get; set; }

    [JsonProperty("roleAces")]
    public List<RoleAce> RoleAces { get; set; }
}

#nullable restore
