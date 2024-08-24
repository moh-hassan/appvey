﻿// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class UpdateCollaborator
{
    [JsonProperty("userId")]
    public int UserId { get; set; }

    [JsonProperty("roleId")]
    public int RoleId { get; set; }
}