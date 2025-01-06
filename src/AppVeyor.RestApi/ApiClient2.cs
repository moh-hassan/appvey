// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using System.Net.Http;
using System.Text;

internal partial class ApiClient
{
    public async Task<ResponseResult> GetApiAsync(string apiUri, CancellationToken ct)
    {
        var response = await Client.GetAsync(apiUri, ct);
        var result = await ResponseResult.CreateAsync(response, apiUri);
        result.Verbose = _verbose;
        return result;
    }

    public async Task<ResponseResult> PostApiAsync(string apiUrl,
        string json,
        CancellationToken ct)
    {
        _ = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));
        _ = json ?? throw new ArgumentNullException(nameof(json));     

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await Client.PostAsync(apiUrl, content, ct);
        var result= await ResponseResult.CreateAsync(response, apiUrl, "post", json);
        result.Verbose = _verbose;
        return result;
    }

    public async Task<ResponseResult> PutApiAsync(string apiUrl,
        string json,
        CancellationToken ct)
    {
        _ = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));
        HttpResponseMessage? response = default;
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        response = await Client.PutAsync(apiUrl, content, ct);
        var result= await ResponseResult
            .CreateAsync(response, apiUrl, "put", json)
            .ConfigureAwait(false);
        result.Verbose = _verbose;
        return result;
    }

    public async Task<ResponseResult> DeleteApiAsync(string apiUrl, CancellationToken ct)
    {
        var response = await Client.DeleteAsync(apiUrl, ct)
            .ConfigureAwait(false);
        var result= await ResponseResult
            .CreateAsync(response, apiUrl, "delete")
            .ConfigureAwait(false);
        result.Verbose = _verbose;
        return result;
    }
    public async Task<ResponseResult> ExecuteApiAsync(string apiUri,
        string method = "get",
        string json = "",
        CancellationToken ct = default)
    {
        switch (method.ToLower())
        {
            case "get":
                return await GetApiAsync(apiUri, ct).ConfigureAwait(false);
            case "post":
                return await PostApiAsync(apiUri, json, ct).ConfigureAwait(false);
            case "put":
                return await PutApiAsync(apiUri, json, ct).ConfigureAwait(false);
            case "delete":
                return await DeleteApiAsync(apiUri, ct).ConfigureAwait(false);
            default:
                throw new NotImplementedException($"Method {method} is not implemented.");
        }
    }
}
