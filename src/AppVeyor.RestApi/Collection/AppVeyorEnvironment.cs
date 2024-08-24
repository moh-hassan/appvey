// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Collection;

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RestApi.Model;

public class EncryptedEnvironmentCollection : IEnumerable<EncryptedEnvironmentVar>
{
    private readonly List<EncryptedEnvironmentVar> _entries = [];

    public EncryptedEnvironmentCollection()
    {
    }

    public EncryptedEnvironmentCollection(IEnumerable<string> envs)
    {
        AddRange(envs);
    }

    private EncryptedEnvironmentVar? Parse(string env)
    {
        var regex = new Regex(@"(?<name>[^:]+):(?<value>(?:""[^""]*""|[^:]+))(?::(?<isEncrypted>true|false))?");

        var match = regex.Match(env);
        if (!match.Success) return default;

        var name = match.Groups["name"].Value;
        var value = match.Groups["value"].Value.Trim('"');
        var isEncrypted = match.Groups["isEncrypted"].Value.ToLower() == "true";
        return new EncryptedEnvironmentVar(name, value, isEncrypted);
    }

    public void Add(string env)
    {
        var entry = Parse(env);
        if (entry != null)
        {
            _entries.Add(entry);
        }
    }

    public void Add(EncryptedEnvironmentVar env)
    {
        _entries.Add(env);
    }

    public void AddRange(IEnumerable<string> envs)
    {
        envs ??= new List<string>();
        foreach (var env in envs)
        {
            Add(env);
        }
    }

    public IEnumerator<EncryptedEnvironmentVar> GetEnumerator() => _entries.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
