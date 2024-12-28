// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

using System.Net;
using Security;

internal class AccountManager
{
    private IEnv Env { get; }

    public AccountManager(IEnv env)
    {
        Env = env;
    }

    public NetworkCredential ToNetworkCredential(string? tok, string? acc)
    {
        var account = acc ?? GetAccount();
        var token = tok ?? GetToken(account);
        var cred = new NetworkCredential(account, token);
        return cred;
    }

    //read the token from the environment variable or the Windows Credential Manager.
    private string GetToken(string? account)
    {
        //1) Check if the token is set in the environment variable.
        WriteLine("Reading 'Token' from Environment Variable.");
        var token = Env.GetToken();
        if (!string.IsNullOrEmpty(token)) return token;

        //2) If the token is not set in the environment variable,
        //check if it is stored in the Windows Credential Manager.
        if (string.IsNullOrEmpty(account)) return string.Empty;

        WriteLine("Reading 'Token' from Windows Credential Manager.");
        var cm = new WindowsCredentialManager();

        if (cm.TryReadToken(account, out token))
            return token;

        throw new AppveyorException("Error Token Exception: Token is null or empty or isn't stored in configuration.");
    }

    //read the account from the environment variable.
    private string GetAccount()
    {
        WriteLine("Reading 'Account' from Environment Variable.");
        var result = Env.GetAccount();
        return result ?? throw new AppveyorException("Account Exception: Account is null or empty."); ;
    }
}
