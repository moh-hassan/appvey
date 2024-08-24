// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

internal static class AccountHelper
{
    public static string GetToken()
    {
        WriteLine("Reading 'Token' from Environment Variable.");
        var token = Environment.GetEnvironmentVariable("APPVEYOR_TOKEN");
        return token
               ?? throw new AppveyorException(
                   "Error Token Exception: Token is null or empty and APPVEYOR_TOKEN env isn't defined.");
    }

    public static string GetAccount(string? account)
    {
        if (!string.IsNullOrEmpty(account)) return account;
        WriteLine("Reading 'Account' from Environment Variable.");
        account = Environment.GetEnvironmentVariable("APPVEYOR_ACCOUNT");

        return account
               ?? throw new AppveyorException(
                   "Account Exception: Account is null or empty and APPVEYOR_ACCOUNT env isn't defined.");
    }
}
