// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class RoleAce
{
    [JsonProperty("roleId")]
    public int RoleId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("isAdmin")]
    public bool IsAdmin { get; set; }

    [JsonProperty("accessRights")]
    public List<AccessRight> AccessRights { get; set; }
}
