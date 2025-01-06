// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test.Commands;

using Api;
using Api.Utility;
using AppVeyor.Test.Extensions;
using FluentAssertions;
using NUnit.Framework.Internal;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class HttpCommandTest
{
    private string _develop = TestCases.develop;
    IEnv _env;

    [OneTimeSetUp]
    public void Setup()
    {
        //To test appveyor production server,You SHOULD:
        //a) setup environment variables: appveyor_account and appveyor_token
        //b) setup testcases for production in TestCases.cs
        //c) Uncomment the next code 

        _env = new DummyEnv();
        _env.StoreAccount(TestCases.accountName, TestCases.token);
        ColorWriter.SetConsole(new ConsoleWrapper());
        ServiceLocator.RegisterService(_env);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _env.Clear();
    }

    [Test]
    public async Task Http_command_with_url_get_project_last_build_branch_test()
    {
        //Arrange
        var url = "/api/projects/moh-hassan/cloudbuilder/branch/master";

        var args = $"http  -v {url}".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        //SharedDataAssertions.ShouldBeEquivalentTo(
        //    "Run Appveyor Rest Api ...",
        //    "http",
        //    "GET /api/projects/moh-hassan/cloudbuilder/branch/master");
        sut.Should().Be(0);
    }

    [Test]
    public async Task Http_command_with_url_Post_AddProject_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var url = "/api/account/moh-hassan/projects";
        var json = """
                 {"repositoryProvider":"gitHub","repositoryName":"test"}
              """;
        var jsonFile = json.WriteToTempFile();
        var args = $"http --json {jsonFile} -v -m post {url}".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        //SharedDataAssertions.ShouldBeEquivalentTo(
        //    "Run Appveyor Rest Api ...",
        //    "http",
        //    "POST /api/account/moh-hassan/projects");
    }
}
