// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using Utility;

public class HttpConnection
{
    public string Token { get; }
    public string Account { get; }
    public string? ProxyAddress { get; set; }
    public string? ProxyUser { get; set; }
    public bool Verbose { get; set; }

    public HttpConnection(string? account, string? token)
    {
        Token = token ?? AccountHelper.GetToken(account)
           ?? throw new AppveyorException("Error Token Exception: Token is null or empty or isn't stored in configuration."); ;
        Account = account ?? AccountHelper.GetAccount()
            ?? throw new AppveyorException("Account Exception: Account is null or empty.");
    }

    public void Deconstruct(out string? proxyAddress, out string? proxyUser)
    {
        proxyAddress = ProxyAddress;
        proxyUser = ProxyUser;
    }
}
