// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class SettingRoot
{
    [JsonProperty("project")]
    public Project Project { get; set; }

    [JsonProperty("settings")]
    public Settings Settings { get; set; }

    [JsonProperty("images")]
    public List<Image> Images { get; set; }

    [JsonProperty("buildClouds")]
    public List<BuildCloud> BuildClouds { get; set; }

    [JsonProperty("defaultImageName")]
    public string DefaultImageName { get; set; }
}
