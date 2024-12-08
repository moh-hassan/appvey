// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using System.Threading.Tasks;

public partial class ApiManager
{
    //----------------------------Teams API--------------------------------

    public async Task<ResponseResult> GetUsersAsync(CancellationToken ct = default)
    {
        var apiUrl = GetUsersUrl(Account);
        return await ApiClient.GetApiAsync(apiUrl, ct);
    }

    public async Task<ResponseResult> GetUserAsync(string userId, CancellationToken ct = default)
    {
        var apiUrl = GetUserUrl(Account, userId);
        return await ApiClient.GetApiAsync(apiUrl, ct);
    }

    public async Task<ResponseResult> AddUserAsync(string json, CancellationToken ct = default)
    {
        var apiUrl = PostUsersUrl(Account);
        return await ApiClient.PostApiAsync(apiUrl, json, ct);
    }

    public async Task<ResponseResult> UpdateUserAsync(string json, CancellationToken ct = default)
    {
        var apiUrl = PutUserUrl(Account);
        return await ApiClient.PutApiAsync(apiUrl, json, ct);
    }

    public async Task<ResponseResult> DeleteUserAsync(string id, CancellationToken ct = default)
    {
        var apiUrl = DelUserUrl(Account, id);
        return await ApiClient.DeleteApiAsync(apiUrl, ct);
    }

    public async Task<ResponseResult> GetCollaboratorsAsync(CancellationToken ct)
    {
        var apiUrl = GetCollaboratorsUrl(Account);
        return await ApiClient.GetApiAsync(apiUrl, ct);
    }

    public async Task<ResponseResult> GetCollaboratorAsync(string userId, CancellationToken ct)
    {
        var apiUrl = GetCollaboratorUrl(Account, userId);
        return await ApiClient.GetApiAsync(apiUrl, ct);
    }

    public async Task<ResponseResult> AddCollaboratorAsync(
        string email,
        string roleId,
        CancellationToken ct)
    {
        var apiUrl = PostCollaboratorsUrl(Account);
        var body = RestBody.AddCollaboratorBody(email, roleId);
        return await ApiClient.PostApiAsync(apiUrl, body, ct);
    }

    public async Task<ResponseResult> UpdateCollaboratorAsync(
        string userId,
        string roleId,
        CancellationToken ct)
    {
        var apiUrl = PutCollaboratorsUrl(Account);
        var body = RestBody.PutCollaboratorsBody(userId, roleId);
        return await ApiClient.PutApiAsync(apiUrl, body, ct);
    }

    public async Task<ResponseResult> DeleteCollaboratorAsync(string userId, CancellationToken ct = default)
    {
        var apiUrl = DelCollaboratorUrl(Account, userId);
        return await ApiClient.DeleteApiAsync(apiUrl, ct);
    }

    public async Task<ResponseResult> GetRolesAsync(CancellationToken ct = default)
    {
        var apiUrl = GetRolesUrl(Account);
        return await ApiClient.GetApiAsync(apiUrl, ct);
    }

    public async Task<ResponseResult> GetRoleAsync(string roleId, CancellationToken ct = default)
    {
        var apiUrl = GetRoleUrl(Account, roleId);
        return await ApiClient.GetApiAsync(apiUrl, ct);
    }
}
