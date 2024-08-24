// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class AddUserBody
{
    [JsonProperty("fullName")]
    public string FullName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("roleId")]
    public int RoleId { get; set; }

    [JsonProperty("generatePassword")]
    public bool GeneratePassword { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("confirmPassword")]
    public string ConfirmPassword { get; set; }
}
