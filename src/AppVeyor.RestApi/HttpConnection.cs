// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using System.Net;
using Utility;
using RestApi.Extensions;

public sealed class HttpConnection
{
    public NetworkCredential AccountCredential { get; set; } = null!;
    public NetworkCredential ProxyCredential { get; set; }
    public string? ProxyAddress { get; set; }

    public bool Verbose { get; set; }
    private AccountManager AccountManager { get; }

    private HttpConnection(IEnv env)
    {
        AccountManager = new AccountManager(env);
        ProxyCredential = new NetworkCredential();
    }

    public static HttpConnection Create(IEnv env,
        string? account = null,
        string? token = null,
        string? proxyAddress = null, string? proxyUser = null, bool verbose = false)
    {
        var connection = new HttpConnection(env)
         .Initialize(account, token, proxyAddress, proxyUser, verbose);
        return connection;
    }

    private HttpConnection Initialize(
        string? account = null,
        string? token = null,
        string? proxyAddress = null,
        string? proxyUser = null, //format-> username:password
        bool verbose = false)
    {
        AccountCredential = AccountManager.ToNetworkCredential(token, account);

        ProxyAddress = proxyAddress;
        if (!string.IsNullOrEmpty(proxyUser))
        {
            var (user, password) = proxyUser.SplitString();
            ProxyCredential = new NetworkCredential(user, password);
        }

        Verbose = verbose;
        WriteLine($"Configured HTTP connection with Account: {AccountCredential.UserName} {account}");
        return this;
    }
}
