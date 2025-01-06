// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

namespace AppVeyor.Api;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Utility;

internal partial class ApiClient : IDisposable
{
    public string AppVeyorBaseUrl { get; }

    internal bool _verbose;
    private HttpClient Client { get; }
    private HttpClientHandler ClientHandler { get; }

    private ApiClient()
    {
        ClientHandler = new HttpClientHandler();
        AppVeyorBaseUrl = GetBaseUrl();
        Client = new HttpClient(ClientHandler)
        {
            BaseAddress = new Uri(AppVeyorBaseUrl),
        };
    }

    public HttpClient GetClient() => Client;

    public static ApiClient Create(HttpConnection httpConnection)
    {
        var client = new ApiClient();
        client.Init(httpConnection);
        return client;
    }

    private void Init(HttpConnection httpConnection)
    {
        //clear cookies
        if (ClientHandler.CookieContainer.Count > 0)
        {
            ClientHandler.CookieContainer = new CookieContainer();
        }

        //clear cache
        Client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true
        };
        // SetupBearer();
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", httpConnection.AccountCredential.Password);
        ConfigureProxy(httpConnection);
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
        SetupAgent();
        _verbose = httpConnection.Verbose;
        if (_verbose)
        {
            WriteInfo($"Success Connection");
        }
    }

    private void SetupAgent()
    {
        var version = new AppVersionInfo().VersionWithNoCommit;
        var productValue = new ProductInfoHeaderValue("appvey", version);
        var commentValue = new ProductInfoHeaderValue("(+https://github.com/moh-hassan/appvey)");
        Client.DefaultRequestHeaders.UserAgent.Add(productValue);
        Client.DefaultRequestHeaders.UserAgent.Add(commentValue);
    }

    private void ConfigureProxy(HttpConnection httpConnection)
    {
        var proxyAddress = httpConnection.ProxyAddress;
        if (string.IsNullOrEmpty(proxyAddress))
        {
            ClientHandler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
            ClientHandler.Proxy = WebRequest.GetSystemWebProxy();
            return;
        }

        var webProxy = new WebProxy(proxyAddress)
        {
            UseDefaultCredentials = false,
            Credentials = httpConnection.ProxyCredential,
        };
       
        ClientHandler.Proxy = webProxy;
        WriteInfo($"Connection is using proxy Server: '{proxyAddress}' with user: '{httpConnection.ProxyCredential.UserName}'");
    }

    private void Dispose(bool disposing)
    {
        if (!disposing) return;
        ClientHandler.Dispose();
        Client.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ApiClient()
    {
        Dispose(false);
    }
}
