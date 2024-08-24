// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Collection;

using System.Collections.Generic;
using System.Text.RegularExpressions;

#pragma warning disable CA2227
public record EnvironmentVar(string name, object value);

public class EnvironmentDictionary : Dictionary<string, object>
{
    public EnvironmentDictionary()
    {
    }

    public EnvironmentDictionary(params string[] items)
    {
        items ??= [];
        foreach (var item in items)
        {
            var envVar = ParseString(item);
            if (envVar != null)
                Add(envVar.name, envVar.value);
        }
    }

    public EnvironmentDictionary(IEnumerable<EnvironmentVar> items)
    {
        foreach (var item in items)
        {
            Add(item.name, item.value);
        }
    }

    private EnvironmentVar? ParseString(string s)
    {
        var pattern = @"^(.*?):\s*""?(.*?)""?$";

        var match = Regex.Match(s, pattern);

        if (match.Success)
        {
            var key = match.Groups[1].Value;
            var value = match.Groups[2].Value;
            return new EnvironmentVar(key, value);
        }

        return null;
    }
}
