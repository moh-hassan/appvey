// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using System.Net.Http;
using System.Text;

internal partial class ApiClient
{
    public async Task<ResponseResult> GetApiAsync(string apiUri, CancellationToken ct)
    {
        if (Verbose)
        {
            WriteInfo($"BaseUrl: {AppVeyorBaseUrl}");
            WriteInfo($"get {apiUri}");
        }

        var response = await Client.GetAsync(apiUri, ct);
        var result = await ResponseResult.CreateAsync(response);
        return result;
    }

    public async Task<ResponseResult> PostApiAsync(string apiUrl, string json, CancellationToken ct)
    {
        _ = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));
        _ = json ?? throw new ArgumentNullException(nameof(json));

        if (Verbose)
        {
            WriteInfo($"BaseUrl: {AppVeyorBaseUrl}");
            WriteInfo($"post {apiUrl}");
            WriteInfo($"Json body:\n{json}");
        }

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await Client.PostAsync(apiUrl, content, ct);
        return await ResponseResult.CreateAsync(response);
    }

    public async Task<ResponseResult> PutApiAsync(string apiUrl, string json, CancellationToken ct)
    {
        _ = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));

        if (Verbose)
        {
            WriteInfo($"put {apiUrl}");
            WriteInfo($"Json body:\n{json}");
        }

        HttpResponseMessage? response = default;
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        response = await Client.PutAsync(apiUrl, content, ct);
        return await ResponseResult.CreateAsync(response);
    }

    public async Task<ResponseResult> DeleteApiAsync(string apiUrl, CancellationToken ct)
    {
        if (Verbose)
        {
            WriteInfo($"BaseUrl: {AppVeyorBaseUrl}");
            WriteInfo($"delete {apiUrl}");
        }

        var response = await Client.DeleteAsync(apiUrl, ct).ConfigureAwait(false);
        return await ResponseResult.CreateAsync(response);
    }

    //public async Task<List<Artifact>?> GetArtifacts(string artifactsUri, CancellationToken ct)
    //{
    //    var artifactsResponse = await Client.GetAsync(artifactsUri, ct);
    //    artifactsResponse.EnsureSuccessStatusCode();
    //    var json = await artifactsResponse.Content.ReadAsStringAsync(ct);
    //    var artifacts = json.ToObject<List<Artifact>>();
    //    return artifacts;
    //}

    //public async Task<int> Download(
    //    string artifactUrl,
    //    string localArtifactPath,
    //    CancellationToken ct)
    //{
    //    var artifactResponse = await Client.GetAsync(artifactUrl, ct);
    //    artifactResponse.EnsureSuccessStatusCode();
    //    var artifactData = await artifactResponse.Content.ReadAsByteArrayAsync(ct);
    //    var size = artifactData.Length;
    //    await File.WriteAllBytesAsync(localArtifactPath, artifactData, ct);
    //    return size;
    //}

    //public async Task<string> GetStringAsync(string url, CancellationToken ct)
    //{
    //    var projectResponse = await Client.GetAsync(url, ct);
    //    projectResponse.EnsureSuccessStatusCode();
    //    var json = await projectResponse.Content.ReadAsStringAsync(ct);
    //    return json;
    //}
}
