// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

namespace AppVeyor.Api;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using RestApi.Extensions;
using Utility;

internal partial class ApiClient : IDisposable
{
    public string AppVeyorBaseUrl { get; }

    internal bool Verbose;
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
        Client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
        SetupBearer(httpConnection.Token);
        var (proxyAddress, proxyUser) = httpConnection;
        ConfigureProxy(proxyAddress, proxyUser);
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

        var version = new AppVersionInfo().VersionWithNoCommit;
        var productValue = new ProductInfoHeaderValue("appvey", version);
        var commentValue = new ProductInfoHeaderValue("(+https://github.com/moh-hassan/appvey)");

        Client.DefaultRequestHeaders.UserAgent.Add(productValue);
        Client.DefaultRequestHeaders.UserAgent.Add(commentValue);
        Verbose = httpConnection.Verbose;
    }

    private void SetupBearer(string token)
    {
        if (string.IsNullOrEmpty(token))
            WriteWarning("Token is null. Skipping Authentication setup.");
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ConfigureProxy(string? proxyAddress, string? proxyUser)
    {
        if (string.IsNullOrEmpty(proxyAddress))
        {
            ClientHandler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
            return;
        }

        WriteInfo($"Connecting to proxy: {proxyAddress}");
        var webProxy = new WebProxy(proxyAddress);

        if (!string.IsNullOrEmpty(proxyUser))
        {
            webProxy.UseDefaultCredentials = false;
            var (user, password) = proxyUser.SplitString();
            webProxy.Credentials = new NetworkCredential(user, password);
            ClientHandler.Proxy = webProxy;
        }
        else
        {
            webProxy.UseDefaultCredentials = true;
        }
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
