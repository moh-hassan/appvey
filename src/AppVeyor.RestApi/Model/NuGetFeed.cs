// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class NuGetFeed
{
    [JsonProperty("nuGetFeedId")]
    public int NuGetFeedId { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("accountId")]
    public int AccountId { get; set; }

    [JsonProperty("projectId")]
    public int ProjectId { get; set; }

    [JsonProperty("isPrivateProject")]
    public bool IsPrivateProject { get; set; }

    [JsonProperty("publishingEnabled")]
    public bool PublishingEnabled { get; set; }

    [JsonProperty("created")]
    public DateTime Created { get; set; }
}
