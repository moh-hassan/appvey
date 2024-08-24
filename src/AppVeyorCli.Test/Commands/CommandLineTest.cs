// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test.Commands;

using Api.Utility;
using Cli.Commands;
using Extensions;
using FluentAssertions;
using RestApi.Extensions;

[TestFixture]
public class CommandLineTest
{
    private string _develop = TestCases.develop;

    [OneTimeSetUp]
    public void Setup()
    {
        //To test appveyor production server,You SHOULD:
        //a) setup environment variables: appveyor_account and appveyor_token
        //b) setup testcases for production in TestCases.cs
        //c) Uncomment the next code 

        //_develop = TestCases.SetupProduction();
        ColorWriter.SetConsole(new ConsoleWrapper());
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        TestCases.SetupTestServer();
    }

    [Test]
    public async Task Get_project_last_build_branch_test()
    {
        //Arrange
        var args = $"project last-build --slug {TestCases.slug} --branch {TestCases.branch} -v"
            .SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Get Project last branch build ...",
            Tag = "project last-build",
            Request = "GET /api/projects/moh-hassan/cloudbuilder/branch/master",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        AppVeyor.Cli.Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
        sut.Should().Be(0);
    }

    [Test]
    public async Task Get_project_last_build_test()
    {
        //Arrange
        var args = $"project last-build --slug {TestCases.slug} -v"
            .SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Get Project last branch build ...",
            Tag = "project last-build",
            Request = "GET /api/projects/moh-hassan/cloudbuilder",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
        sut.Should().Be(0);
    }

    [Test]
    public async Task Get_projects_test()
    {
        //Arrange
        var argument = "project list -v";
        var args = argument.SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Get projects ...",
            Tag = "project list",
            Request = "GET /api/account/moh-hassan/projects",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        sut.Should().Be(0);
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
        argument.Should().StartWith(expectedExecutionInfo.Tag);
    }


    [Test]
    public async Task Get_project_build_by_version_test()
    {
        //Arrange
        var args = $"project build-version --slug {TestCases.slug} {TestCases.version}"
            .SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Get project build by version ...",
            Tag = "project build-version",
            Request = $"GET /api/projects/moh-hassan/cloudbuilder/build/{TestCases.version}",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
        sut.Should().Be(0);
    }

    [Test]
    public async Task Get_project_history_test()
    {
        //Arrange
        var args = $"project history --slug {TestCases.slug} --branch {TestCases.branch}".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Get project history ...",
            Tag = "project history",
            Request = "GET /api/projects/moh-hassan/cloudbuilder/history",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
        sut.Should().Be(0);
    }


    [Test]
    public async Task Get_project_deployments_test()
    {
        //Arrange
        var args = $"project deploy --slug {TestCases.slug} -v".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Project deployments ...",
            Tag = "project deploy",
            Request = "GET /api/projects/moh-hassan/cloudbuilder/deployments",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Get_project_settings_test()
    {
        //Arrange
        var args = $"project setting --slug {TestCases.slug}".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Get project settings ...",
            Tag = "project settings",
            Request = "GET /api/projects/moh-hassan/cloudbuilder/settings",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Get_project_settings_in_YAML_test()
    {
        //Arrange
        var args = $"project yaml --slug {TestCases.slug}".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Get project settings in YAML ...",
            Tag = "project yaml",
            Request = "GET /api/projects/moh-hassan/cloudbuilder/settings/yaml",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Get_project_environment_variables_test()
    {
        //Arrange
        var args = $"project env --slug {TestCases.slug}".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Get project environment variables ...",
            Tag = "project env",
            Request = "GET /api/projects/moh-hassan/cloudbuilder/settings/environment-variables",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Post_AddProject_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"project add --provider {TestCases.repositoryProvider}  {TestCases.repositoryName}".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Add project ...",
            Tag = "project add",
            Request = "POST /api/account/moh-hassan/projects",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task UpdateProjectEnvironmentVariables_using_env_list_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");
        //Arrange
        var args = ($"project update env --slug {TestCases.slug} "
                    + "api_key:very-secret-key-encrypted:true var1:new-value")
            .SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Update project environment variables ...",
            Tag = "project update env",
            Request = "PUT /api/projects/moh-hassan/cloudbuilder/settings/environment-variables",
        };

        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
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
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Update project environment variables ...",
            Tag = "project update env",
            Request = "PUT /api/projects/moh-hassan/cloudbuilder/settings/environment-variables",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Put_update_project_build_number_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"project update build-number 35 --slug {TestCases.slug} -v".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Update project build number ...",
            Tag = "project update build-number",
            Request = "PUT /api/projects/moh-hassan/cloudbuilder/settings/build-number",
        };

        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Delete_project_build_cache_test()
    {
        //Arrange
        var args = $"project delete-cache --slug {TestCases.slug} ".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Delete project build cache ...",
            Tag = "project delete-cache",
            Request = "DELETE /api/projects/moh-hassan/cloudbuilder/buildcache",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert

        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Delete_project_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"project delete --slug {TestCases.slug} ".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Delete project ...",
            Tag = "project delete",
            Request = "DELETE /api/projects/moh-hassan/cloudbuilder",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
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
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Update project settings in YAML ...",
            Tag = "project update yaml",
            Request = "PUT /api/projects/moh-hassan/cloudbuilder/settings/yaml",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Start_build_most_recent_commit_test()
    {
        //Arrange
        var args = $"build start recent --slug {TestCases.slug} --branch {TestCases.branch} api_key:very-secret-key-encrypted var1:new-value".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Start build of branch most recent commit ...",
            Tag = "build start recent",
            Request = "POST /api/account/moh-hassan/builds",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Start_build_of_specific_branch_commit_test()
    {
        //Arrange
        var args = $"build start commit --slug {TestCases.slug} --branch {TestCases.branch}  {TestCases.commitId}".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Start build of specific branch commit ...",
            Tag = "build start commit",
            Request = "POST /api/account/moh-hassan/builds",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Re_run_build_test()
    {
        //Arrange
        var args = $"build rerun  {TestCases.buildId}".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Re-run build ...",
            Tag = "build rerun",
            Request = "PUT /api/account/moh-hassan/builds",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Start_build_of_pull_request_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"build start pr --slug {TestCases.slug} {TestCases.pullRequestId}"
            .SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Start build of Pull Request ...",
            Tag = "build start pr",
            Request = "POST /api/account/moh-hassan/builds",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Cancel_build_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"build cancel --slug {TestCases.slug} {TestCases.version} -v"
            .SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Cancel build ...",
            Tag = "build cancel",
            Request = $"DELETE /api/builds/moh-hassan/cloudbuilder/{TestCases.version}",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Delete_builds_test()
    {
        if (_develop == "0") Assert.Ignore("This test is ignored in production mode");

        //Arrange
        var args = $"build delete {TestCases.buildId}"
            .SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Delete build ...",
            Tag = "build delete",
            Request = "DELETE /api/account/moh-hassan/builds/50127590",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }

    [Test]
    public async Task Download_build_log_test()
    {
        //Arrange
        var args = $"build download log --job-id {TestCases.jobId} --slug {TestCases.slug}".SplitArgs();
        var expectedExecutionInfo = new ExecutionInfo
        {
            Title = "Download build log ...",
            Tag = "build download log",
            Request = $"GET /api/buildjobs/{TestCases.jobId}/log",
        };
        //Act
        var sut = await Program.Main(args);
        //Assert
        Assert.That(sut, Is.EqualTo(0));
        Program.ExecutionInfo.Should().BeEquivalentTo(expectedExecutionInfo);
    }
}
