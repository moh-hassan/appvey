// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.RestApi.Extensions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class JsonExtensions
{
    public static T? ToObject<T>(this string json)
    {
        return string.IsNullOrWhiteSpace(json)
             ? default
             : JsonConvert.DeserializeObject<T>(json);
    }

    public static JObject ToJObject(this string json)
    {
        return string.IsNullOrWhiteSpace(json) ? new JObject() : JObject.Parse(json);
    }

    public static string ToJson(this object obj, bool ident = false)
    {
        _ = obj ?? throw new ArgumentNullException(nameof(obj));
        var formatting = ident ? Formatting.Indented : Formatting.None;
        var setting = new JsonSerializerSettings
        {
            //DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = formatting,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };
        return JsonConvert.SerializeObject(obj, setting);
    }

    public static string JsonFormat(this string json)
    {
        try
        {
            var formattedJson = JToken.Parse(json).ToString(Formatting.Indented);
            return formattedJson;
        }
        catch
        {
            //not json
            return json;
        }
    }

    public static bool IsValidJson(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;

        json = json.Trim();

        if ((json.StartsWith("{") && json.EndsWith("}")) || // For object
            (json.StartsWith("[") && json.EndsWith("]")))   // For array
        {
            try
            {
                _ = JToken.Parse(json);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        return false;
    }
}



