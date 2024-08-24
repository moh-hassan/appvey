// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class UpdateUserBody
{
    [JsonProperty("userId")]
    public int UserId { get; set; }

    [JsonProperty("fullName")]
    public string FullName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("password")]
    public object Password { get; set; }

    [JsonProperty("roleId")]
    public int RoleId { get; set; }

    [JsonProperty("successfulBuildNotification")]
    public string SuccessfulBuildNotification { get; set; }

    [JsonProperty("failedBuildNotification")]
    public string FailedBuildNotification { get; set; }

    [JsonProperty("notifyWhenBuildStatusChangedOnly")]
    public bool NotifyWhenBuildStatusChangedOnly { get; set; }

    [JsonProperty("successfulDeploymentNotification")]
    public string SuccessfulDeploymentNotification { get; set; }

    [JsonProperty("failedDeploymentNotification")]
    public string FailedDeploymentNotification { get; set; }
}
