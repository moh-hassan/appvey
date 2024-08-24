// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class AccessRight
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("allowed")]
    public bool Allowed { get; set; }
}
