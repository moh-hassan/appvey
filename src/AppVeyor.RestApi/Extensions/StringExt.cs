// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.RestApi.Extensions;

using System;
using System.Text.RegularExpressions;
using System.Web;

public static class StringExt
{
    public static string AsString(this ICollection<string> args, string separator = ",")
    {
        if (args == null || !args.Any())
            return string.Empty;
        return string.Join(separator, args);
    }

    public static (string key, string value) SplitString(this string text, char separator)
    {
        var pattern = @$"^(.*?){separator}\s*""?(.*?)""?$";
        var match = Regex.Match(text, pattern);
        return match.Success
            ? (match.Groups[1].Value, match.Groups[2].Value)
            : (text, string.Empty);
    }

    public static (string key, string value) SplitString(this string text)
    {
        //separator can be either : or =
        var pattern = @"^(.*?)[:=]\s*""?(.*?)""?$";
        var match = Regex.Match(text, pattern);
        return match.Success
            ? (match.Groups[1].Value, match.Groups[2].Value)
            : (text, string.Empty);
    }
    public static string? FullException(this Exception ex)
    {
        if (ex == null) return null;
        var msg = ex.Message;
        if (ex.InnerException != null)
        {
            msg += "\nInnerException: " + FullException(ex.InnerException);
        }

        return msg;
    }

    public static string Q(this string str) => $"\"{str}\"";
    public static string AddQueryString(this string uri, string name, object? obj)
    {
        var value = obj?.ToString();
        if (string.IsNullOrEmpty(value)) return uri;
        var separator = uri.Contains("?") ? "&" : "?";

        var encodedName = HttpUtility.UrlEncode(name).Replace("+", "%20");
        var encodedValue = HttpUtility.UrlEncode(value).Replace("+", "%20");
        return $"{uri}{separator}{encodedName}={encodedValue}";
    }

    public static string AddQueryString(this string uri, Dictionary<string,object>  values)
    {
        if (values == null || values.Count == 0) return uri;
        uri = values.Aggregate(uri, (current, kvp)
            => current.AddQueryString(kvp.Key, kvp.Value.ToString()));
        return uri;
    }
}
