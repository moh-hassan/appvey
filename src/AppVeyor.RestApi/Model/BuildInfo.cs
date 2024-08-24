// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Model;

#nullable disable
using AppVeyor.RestApi.Model;
using Newtonsoft.Json;

public class BuildInfo
{
    [JsonProperty("project")]
    public Project Project { get; set; }

    [JsonProperty("build")]
    public Build Build { get; set; }
}

#nullable restore


