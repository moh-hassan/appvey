// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Model;

using Newtonsoft.Json;
#nullable disable
public class Role
{
    [JsonProperty("roleId")]
    public int RoleId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("isSystem")]
    public bool IsSystem { get; set; }

    [JsonProperty("created")]
    public DateTime Created { get; set; }
}
#nullable restore
