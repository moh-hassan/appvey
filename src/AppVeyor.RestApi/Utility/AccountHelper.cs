// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

using Security;

internal static class AccountHelper
{
    //read the token from the environment variable or the Windows Credential Manager.
    public static string? GetToken(string? account)
    {
        //1) Check if the token is set in the environment variable.
        WriteLine("Reading 'Token' from Environment Variable.");
        var token = Env.GetToken();
        if (!string.IsNullOrEmpty(token)) return token;

        //2) If the token is not set in the environment variable,
        //check if it is stored in the Windows Credential Manager.
        if (string.IsNullOrEmpty(account)) return string.Empty;

        WriteLine("Reading 'Token' from Windows Credential Manager.");
        var cm = new CredentialManager();

        if (cm.TryRetrieveToken(account, out token))
            return token;

        return null;
    }

    //read the account from the environment variable.
    public static string? GetAccount()
    {
        WriteLine("Reading 'Account' from Environment Variable.");
        var result = Env.GetAccount();
        return result;
    }
}
