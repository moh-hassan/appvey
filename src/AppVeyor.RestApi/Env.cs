// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Env
{
    private static string AppveyorTokenName = "APPVEYOR_TOKEN";
    private static string AppveyorAccountName = "APPVEYOR_ACCOUNT";

    // StoreEnv("APPVEYOR_ACCOUNT", Account)
    public static void StoreEnv(string key, string? value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));
        if (string.IsNullOrEmpty(value)) return;
        Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.User);
    }

    public static void StoreToken(string? value)
    {
        StoreEnv(AppveyorTokenName, value);
    }

    public static void StoreAccount(string? value)
    {
        StoreEnv(AppveyorAccountName, value);
    }

    public static string? GetToken()
    {
        return GetEnv(AppveyorTokenName);
    }

    public static string? GetAccount()
    {
        var result= GetEnv(AppveyorAccountName);
        return result;
    }

    public static string? GetEnv(string key)
    {
        return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
    }

    public static void Remove(string key)
    {
        Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.User);
    }
}
