// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

using AppVeyor.Api.Utility;
using AppVeyor.Test;
using AppVeyor.Test.Mocks;

[SetUpFixture]
public class TestSetup
{
    [OneTimeSetUp]
    public void Setup()
    {
        var develop = TestCases.SetupTestServer();
        ColorWriter.WriteLine($"Develop env: {develop}");
        ApiService.StartServer();
        ColorWriter.SetConsole(new DummyConsole());
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ApiService.StopServer();
    }
}
