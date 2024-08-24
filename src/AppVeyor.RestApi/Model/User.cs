// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Model;

using Newtonsoft.Json;

#nullable disable
public class User
{
    [JsonProperty("accountId")]
    public int AccountId { get; set; }

    [JsonProperty("accountName")]
    public string AccountName { get; set; }

    [JsonProperty("isOwner")]
    public bool IsOwner { get; set; }

    [JsonProperty("isCollaborator")]
    public bool IsCollaborator { get; set; }

    [JsonProperty("userId")]
    public int UserId { get; set; }

    [JsonProperty("fullName")]
    public string FullName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("roleId")]
    public int RoleId { get; set; }

    [JsonProperty("roleName")]
    public string RoleName { get; set; }

    [JsonProperty("twoFactorAuthEnabled")]
    public bool TwoFactorAuthEnabled { get; set; }

    [JsonProperty("pageSize")]
    public int PageSize { get; set; }

    [JsonProperty("created")]
    public DateTime Created { get; set; }
}
#nullable restore
