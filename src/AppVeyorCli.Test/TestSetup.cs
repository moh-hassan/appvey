// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

using AppVeyor.Api;
using AppVeyor.Api.Utility;
using AppVeyor.Test;
using AppVeyor.Test.Mocks;

[SetUpFixture]
public class TestSetup
{
    private string? _account;
    private string? _token;

    [OneTimeSetUp]
    public void Setup()
    {
        StoreEnvironment();
        SetTestAccount();
        var develop = TestCases.SetupTestServer();
        ColorWriter.WriteLine($"Develop env: {develop}");
        ApiService.StartServer();
        ColorWriter.SetConsole(new DummyConsole());
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ApiService.StopServer();
        RestoreEnvironment();
    }

    private void StoreEnvironment()
    {
        _account = Env.GetAccount();
        _token = Env.GetToken();
    }

    private void RestoreEnvironment()
    {
        if (_account != null)
        {
            Env.StoreAccount(_account);
        }

        if (_token != null)
        {
            Env.StoreToken(_token);
        }
    }

    private void SetTestAccount()
    {
        var account = Env.GetAccount();
        if (string.IsNullOrEmpty(account))
        {
            Env.StoreAccount("moh-hassan");
        }

        var token = Env.GetToken();
        if (string.IsNullOrEmpty(token))
        {
            Env.StoreToken("secret");
        }
    }
}
