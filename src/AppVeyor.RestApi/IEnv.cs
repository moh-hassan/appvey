// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

public interface IEnv
{
    void StoreToken(string value);
    void StoreAccount(string value);
    void StoreAccount(string account,string token);
    string? GetToken();
    string? GetAccount();
    void Remove(string key);
    void RemoveAccount();
    void RemoveToken();
    void Clear();
}
