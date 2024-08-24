// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class EnvironmentVariable
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("value")]
    public EnvironmentValue Value { get; set; }
}
