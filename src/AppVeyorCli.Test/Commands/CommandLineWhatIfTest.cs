// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test.Commands;

using Api;
using Api.Utility;
using Extensions;
using FluentAssertions;

[TestFixture]
public class CommandLineWhatIfTest
{
    private string _develop = TestCases.develop;
    IEnv _env;

    [SetUp]
    public void Setup()
    {
        //direct console to stringwriter
       //  Console.SetOut(new StringWriter());
    }

    [TearDown]
    public void TearDown()
    {
     //   _env.Clear();
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
    }

    [OneTimeSetUp]
    public void OneTimeSetup0()
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
    public void OneTimeTearDown()
    {
        _env.Clear();
    }

    [Test]
    public async Task Get_project_last_build_branch_test()
    {
        //Arrange
        var args = $"project last-build --slug {TestCases.slug} --branch {TestCases.branch} --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/projects/moh-hassan/cloudbuilder/branch/master");        
        sut.Should().Be(0);
    }


    [Test]
    public async Task Get_project_last_build_test()
    {
        //Arrange
        var args = $"project last-build --slug {TestCases.slug} -w"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/projects/moh-hassan/cloudbuilder");  
        sut.Should().Be(0);
    }

    [Test]
    public async Task Get_projects_test()
    {
        //Arrange
        var argument = "project list -w";
        var args = argument.SplitArgs();

        //Act            
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/account/moh-hassan/projects");
        sut.Should().Be(0);
        
    }


    [Test]
    public async Task Get_project_build_by_version_test()
    {
        //Arrange
        var args = $"project build-version --slug {TestCases.slug} {TestCases.version} -w"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/projects/moh-hassan/cloudbuilder/build/1.0.0-dev-1");
        sut.Should().Be(0);
    }

    [Test]
    public async Task Get_project_history_test()
    {
        //Arrange
        var args =
            $"project history --slug {TestCases.slug} --branch {TestCases.branch} -w".SplitArgs();
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/projects/moh-hassan/cloudbuilder/history");
        sut.Should().Be(0);
    }


    [Test]
    public async Task Get_project_deployments_test()
    {
        //Arrange
        var args = $"project deploy --slug {TestCases.slug} -w".SplitArgs();
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/projects/moh-hassan/cloudbuilder/deployments");
        Assert.That(sut, Is.EqualTo(0));       
    }

    [Test]
    public async Task Get_project_settings_test()
    {
        //Arrange
        var args = $"project setting --slug {TestCases.slug} --what-if".SplitArgs();
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/projects/moh-hassan/cloudbuilder/settings");
        Assert.That(sut, Is.EqualTo(0));
    }

    [Test]
    public async Task Get_project_settings_in_YAML_test()
    {
        //Arrange
        var args = $"project yaml --slug {TestCases.slug} --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/projects/moh-hassan/cloudbuilder/settings/yaml");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Get_project_environment_variables_test()
    {
        //Arrange
        var args = $"project env --slug {TestCases.slug} --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/projects/moh-hassan/cloudbuilder/settings/environment-variables");
        Assert.That(sut, Is.EqualTo(0));        
    }

    [Test]
    public async Task Post_AddProject_test()
    {
        //Arrange
        var args = $"project add --provider {TestCases.repositoryProvider}  {TestCases.repositoryName} --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: POST /api/account/moh-hassan/projects");
        Assert.That(sut, Is.EqualTo(0));
        
    }

    [Test]
    public async Task UpdateProjectEnvironmentVariables_using_env_list_test()
    {
        //Arrange
        var args = ($"project update env --slug {TestCases.slug} --what-if "
                    + "api_key:very-secret-key-encrypted:true var1:new-value")
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: PUT /api/projects/moh-hassan/cloudbuilder/settings/environment-variables");
        Assert.That(sut, Is.EqualTo(0));
        
    }

    [Test]
    public async Task UpdateProjectEnvironmentVariables_using_json_test()
    {
        //Arrange
        var json = "{}";
        var jsonFile = json.WriteToTempFile();
        var args =
            $"project update env --slug {TestCases.slug} --json {jsonFile} --what-if".SplitArgs();
    
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: PUT /api/projects/moh-hassan/cloudbuilder/settings/environment-variables");
        Assert.That(sut, Is.EqualTo(0));
       
    }

    [Test]
    public async Task Put_update_project_build_number_test()
    {
        //Arrange
        var args =
            $"project update build-number 35 --slug {TestCases.slug} --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: PUT /api/projects/moh-hassan/cloudbuilder/settings/build-number");
        Assert.That(sut, Is.EqualTo(0));
       
    }

    [Test]
    public async Task Delete_project_build_cache_test()
    {
        //Arrange
        var args = $"project delete-cache --slug {TestCases.slug} --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: DELETE /api/projects/moh-hassan/cloudbuilder/buildcache");
        Assert.That(sut, Is.EqualTo(0));
        
    }

    [Test]
    public async Task Delete_project_test()
    {
        //Arrange
        var args = $"project delete --slug {TestCases.slug}  --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: DELETE /api/projects/moh-hassan/cloudbuilder");
        Assert.That(sut, Is.EqualTo(0));
        
    }

    [Test]
    public async Task Update_project_setting_yaml_test()
    {
        //Arrange
        var contents = "version: 1.0.0";
        var yamlFile = contents.WriteToTempFile();
        var args = $"project update yaml --slug {TestCases.slug} {yamlFile}  --what-if"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: PUT /api/projects/moh-hassan/cloudbuilder/settings/yaml");
        Assert.That(sut, Is.EqualTo(0));
       
    }


    [Test]
    public async Task Start_build_most_recent_commit_test()
    {
        //Arrange
        var args = $"build start recent --slug {TestCases.slug} --branch {TestCases.branch} api_key:very-secret-key-encrypted var1:new-value  --what-if"
            .SplitArgs();

        string expectedBody = """
            {"accountName":"moh-hassan","projectSlug":"cloudbuilder","branch":"master","environmentVariables":{"api_key":"very-secret-key-encrypted","var1":"new-value"}}        
            """;
        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: POST /api/account/moh-hassan/builds");
        Logger.Text.Should().Contain(expectedBody.Trim());
        Assert.That(sut, Is.EqualTo(0));
       
    }


    [Test]
    public async Task Start_build_of_specific_branch_commit_test()
    {
        //Arrange
        var args = $"build start commit --slug {TestCases.slug} --branch {TestCases.branch}  {TestCases.commitId}  --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: POST /api/account/moh-hassan/builds");
        Assert.That(sut, Is.EqualTo(0));
        
    }

    [Test]
    public async Task Re_run_build_test()
    {
        //Arrange
        var args = $"build rerun  {TestCases.buildId}  --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: POST /api/account/moh-hassan/builds");
        Assert.That(sut, Is.EqualTo(0));
       
    }

    [Test]
    public async Task Start_build_of_pull_request_test()
    {
        //Arrange
        var args =
            $"build start pr --slug {TestCases.slug} {TestCases.pullRequestId}  --what-if"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: POST /api/account/moh-hassan/builds");
        Assert.That(sut, Is.EqualTo(0));
       
    }

    [Test]
    public async Task Cancel_build_test()
    {
        //Arrange
        var args = $"build cancel --slug {TestCases.slug} {TestCases.version}  --what-if"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: DELETE /api/builds/moh-hassan/cloudbuilder/1.0.0-dev-1");
        Assert.That(sut, Is.EqualTo(0));
        
    }

    [Test]
    public async Task Delete_builds_test()
    {
        //Arrange
        var args = $"build delete {TestCases.buildId}  --what-if"
            .SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: DELETE /api/account/moh-hassan/builds/50127590");
        Assert.That(sut, Is.EqualTo(0));
        
    }

    [Test]
    public async Task Download_build_log_test()
    {
        //Arrange
        var args =
            $"build download log --job-id {TestCases.jobId} --slug {TestCases.slug}  --what-if".SplitArgs();

        //Act
        var sut = await Bootstrapper.StartAsync(args);
        //Assert
        Logger.Text.Should().Contain("Http Request: GET /api/buildjobs/3akc42kgf8d7fvyt/log");
        Assert.That(sut, Is.EqualTo(0));
       
    }

}
