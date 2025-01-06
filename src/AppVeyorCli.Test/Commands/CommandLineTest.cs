// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test.Commands;

using System.Net;
using Api;
using Api.Utility;
using Extensions;
using FluentAssertions;

[TestFixture]
public class CommandLineTest
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
    public async Task Get_project_last_build_branch_test()
    {
        //Arrange
        var args = $"project last-build --slug {TestCases.slug} --branch {TestCases.branch} -v"
            .SplitArgs();
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("GET /api/projects/moh-hassan/cloudbuilder/branch/master");
        sut.Should().Be(0);
    }

    [Test]
    public async Task Get_project_last_build_test()
    {
        //Arrange
        var args = $"project last-build --slug {TestCases.slug} -v"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Console.WriteLine(Bootstrapper.ResponseResult.ResponseString);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("GET /api/projects/moh-hassan/cloudbuilder");
        sut.Should().Be(0);
    }

    [Test]
    public async Task Get_projects_test()
    {
        //Arrange
        var argument = "project list -v";
        var args = argument.SplitArgs();

        //Act            
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        sut.Should().Be(0);
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("GET /api/account/moh-hassan/projects");
    }


    [Test]
    public async Task Get_project_build_by_version_test()
    {
        //Arrange
        var args = $"project build-version --slug {TestCases.slug} {TestCases.version}"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be($"GET /api/projects/moh-hassan/cloudbuilder/build/{TestCases.version}");      
        sut.Should().Be(0);
    }

    [Test]
    public async Task Get_project_history_test()
    {
        //Arrange
        var args = $"project history --slug {TestCases.slug} --branch {TestCases.branch}".SplitArgs();
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.Url.Should()
            .Be("/api/projects/moh-hassan/cloudbuilder/history?recordsNumber=20&branch=master");
        sut.Should().Be(0);
    }


    [Test]
    public async Task Get_project_deployments_test()
    {
        //Arrange
        var args = $"project deploy --slug {TestCases.slug} -v".SplitArgs();
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        Console.WriteLine(Bootstrapper.ResponseResult.Url);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.Url.Should()
            .Be("/api/projects/moh-hassan/cloudbuilder/deployments?recordsNumber=20");
    }

    [Test]
    public async Task Get_project_settings_test()
    {
        //Arrange
        var args = $"project setting --slug {TestCases.slug}".SplitArgs();
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("GET /api/projects/moh-hassan/cloudbuilder/settings");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Get_project_settings_in_YAML_test()
    {
        //Arrange
        var args = $"project yaml --slug {TestCases.slug}".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("GET /api/projects/moh-hassan/cloudbuilder/settings/yaml");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Get_project_environment_variables_test()
    {
        //Arrange
        var args = $"project env --slug {TestCases.slug}".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("GET /api/projects/moh-hassan/cloudbuilder/settings/environment-variables");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Post_AddProject_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"project add --provider {TestCases.repositoryProvider}  {TestCases.repositoryName} -v".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("POST /api/account/moh-hassan/projects");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task UpdateProjectEnvironmentVariables_using_env_list_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");
        //Arrange
        var args = ($"project update env --slug {TestCases.slug} "
                    + "api_key:very-secret-key-encrypted:true var1:new-value")
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("PUT /api/projects/moh-hassan/cloudbuilder/settings/environment-variables");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task UpdateProjectEnvironmentVariables_using_json_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var json = """
                   [
                     {
                       "name": "api_key",
                       "value": {
                         "isEncrypted": true,
                         "value": "very-secret-key-encrypted"
                       }
                     },
                     {
                       "name": "var1",
                       "value": {
                         "isEncrypted": false,
                         "value": "new-value"
                       }
                     }
                   ]
                   """;
        //write json to tempfile
        var tempFile = json.WriteToTempFile();

        var args = $"project update env --slug {TestCases.slug} --json {tempFile}"
            .SplitArgs();
       
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("PUT /api/projects/moh-hassan/cloudbuilder/settings/environment-variables");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Put_update_project_build_number_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"project update build-number 35 --slug {TestCases.slug} -v".SplitArgs();
        
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("PUT /api/projects/moh-hassan/cloudbuilder/settings/build-number"); 
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Delete_project_build_cache_test()
    {
        //Arrange
        var args = $"project delete-cache --slug {TestCases.slug} ".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("DELETE /api/projects/moh-hassan/cloudbuilder/buildcache");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Delete_project_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"project delete --slug {TestCases.slug} ".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("DELETE /api/projects/moh-hassan/cloudbuilder");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Update_project_setting_yaml_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var contents = "version: 1.0.0";
        var yamlFile = contents.WriteToTempFile();
        var args = $"project update yaml --slug {TestCases.slug} {yamlFile}"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("PUT /api/projects/moh-hassan/cloudbuilder/settings/yaml");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Start_build_most_recent_commit_test()
    {
        //Arrange
        var args = $"build start recent --slug {TestCases.slug} --branch {TestCases.branch} api_key:very-secret-key-encrypted var1:new-value".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("POST /api/account/moh-hassan/builds");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Start_build_of_specific_branch_commit_test()
    {
        //Arrange
        var args = $"build start commit --slug {TestCases.slug} --branch {TestCases.branch}  {TestCases.commitId}".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("POST /api/account/moh-hassan/builds");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Re_run_build_test()
    {
        //Arrange
        var args = $"build rerun  {TestCases.buildId}".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("PUT /api/account/moh-hassan/builds");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Start_build_of_pull_request_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"build start pr --slug {TestCases.slug} {TestCases.pullRequestId}"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("POST /api/account/moh-hassan/builds");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Cancel_build_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"build cancel --slug {TestCases.slug} {TestCases.version} -v"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be($"DELETE /api/builds/moh-hassan/cloudbuilder/{TestCases.version}");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Delete_builds_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"build delete {TestCases.buildId}"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be("DELETE /api/account/moh-hassan/builds/50127590");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Download_build_log_test()
    {
        //Arrange
        var args = $"build download log --job-id {TestCases.jobId} --slug {TestCases.slug}".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Bootstrapper.ResponseResult.IsSuccess.Should().BeTrue();
        Bootstrapper.ResponseResult.StatusCode.Should().Be(HttpStatusCode.OK);
        Bootstrapper.ResponseResult.HttpRequest.Should()
            .Be($"GET /api/buildjobs/{TestCases.jobId}/log");
        Assert.That(sut, Is.EqualTo(0));        
    }
}
