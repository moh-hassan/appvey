// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class Image
{
    [JsonProperty("buildWorkerImageId")]
    public int BuildWorkerImageId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("osType")]
    public string OsType { get; set; }

    [JsonProperty("buildCloudName")]
    public string BuildCloudName { get; set; }
}
