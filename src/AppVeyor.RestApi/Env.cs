// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;
using System;
using System.Collections.Generic;

internal class Env : IEnv
{
    private const string AppveyorTokenName = "APPVEYOR_TOKEN";
    private const string AppveyorAccountName = "APPVEYOR_ACCOUNT";

    public void StoreToken(string value)
    {
        StoreEnv(AppveyorTokenName, value);
    }

    public void StoreAccount(string? value)
    {
        StoreEnv(AppveyorAccountName, value);
    }

    public void StoreAccount(string account, string token)
    {
        StoreAccount(account);
        StoreToken(token);
    }

    public string? GetToken()
    {
        return GetEnv(AppveyorTokenName);
    }

    public string? GetAccount()
    {
        var result = GetEnv(AppveyorAccountName);
        return result;
    }

    public void Remove(string key)
    {
        Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.User);
    }

    public void RemoveAccount()
    {
        Remove(AppveyorAccountName);
    }

    public void RemoveToken()
    {
        Remove(AppveyorTokenName);
    }

    public void Clear()
    {
        //do nothing
    }

    private void StoreEnv(string key, string? value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));
        if (string.IsNullOrEmpty(value)) return;
        Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.User);
    }

    private string? GetEnv(string key)
    {
        return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
    }

    public override string ToString()
    {
        return $"Env";
    }

    public bool IsExists(string account)
    {
        return GetAccount() == account;
    }
}

/// <summary>
/// Dummy implementation of IEnv for testing
/// </summary>
internal class DummyEnv : IEnv
{
    private static Dictionary<string, string> _env = new();
    private const string AppveyorTokenName = "APPVEYOR_TOKEN";
    private const string AppveyorAccountName = "APPVEYOR_ACCOUNT";

    public DummyEnv()
    {
#if DEBUG
        Console.WriteLine("using dummy env");
#endif
    }

    public void StoreToken(string value)
    {
        _env[AppveyorTokenName] = value;
    }

    public void StoreAccount(string value)
    {
        _env["APPVEYOR_ACCOUNT"] = value;
    }

    public void StoreAccount(string account, string token)
    {
        StoreAccount(account);
        StoreToken(token);
    }

    public string? GetToken()
    {
        _env.TryGetValue(AppveyorTokenName, out var token);
        return token;
    }

    public string? GetAccount()
    {
        _env.TryGetValue("APPVEYOR_ACCOUNT", out var account);
        return account;
    }

    public void Remove(string key)
    {
        _env.Remove(key);
    }

    public void RemoveAccount()
    {
        Remove(AppveyorAccountName);
    }

    public void RemoveToken()
    {
        Remove(AppveyorTokenName);
    }

    public void Clear()
    {
        _env.Clear();
    }

    public override string ToString()
    {
        return $"account={_env["APPVEYOR_ACCOUNT"]} token= {_env["APPVEYOR_TOKEN"]}";
    }

    public bool IsExists(string account)
    {
        return GetAccount() == account;
    }
}
