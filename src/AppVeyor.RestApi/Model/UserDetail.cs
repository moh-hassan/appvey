// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Model;

using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable
public class UserDetail
{
    [JsonProperty("user")]
    public User User { get; set; }

    [JsonProperty("roles")]
    public List<Role> Roles { get; set; }
}
#nullable restore


