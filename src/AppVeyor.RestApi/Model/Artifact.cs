namespace AppVeyor.Api.Model;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Artifact
{
    [JsonProperty("fileName")]
    public string FileName { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("size")]
    public int Size { get; set; }

    [JsonProperty("created")]
    public DateTime Created { get; set; }
}

