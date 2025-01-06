// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Collection;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using AppVeyor.RestApi.Extensions;

#pragma warning disable CA2227
public record EnvironmentVar(string name, object value);

public class EnvironmentDictionary : Dictionary<string, object>
{
    public EnvironmentDictionary()
    {
    }

    public EnvironmentDictionary(params string[] items)
    {
        if (items == null || items.Length == 0) return;
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
        var (key, value) = s.SplitString();
        return new EnvironmentVar(key, value);
    }
}
