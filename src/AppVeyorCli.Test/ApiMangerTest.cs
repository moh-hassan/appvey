// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test;

using System.Net;
using System.Threading.Tasks;
using Api;
using Api.Collection;
using Extensions;

public class ApiMangerTest
{
    private ApiClient _apiClient;
    private ApiManager _apiManager;
    private CancellationToken _ct = default;

    [OneTimeSetUp]
    public void Setup()
    {
        var httpConnection = new HttpConnection
        {
            Verbose = true
        };
        _apiClient = ApiClient.Create(httpConnection);
        _apiManager = new ApiManager(httpConnection);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _apiClient.Dispose();
        _apiManager.Dispose();
    }

    //-----------------------apiManager-------------------
    [Test]
    public async Task Get_projects_test()
    {
        //Act
        var sut = await _apiManager.GetProjectsAsync(_ct);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.IsSuccess, Is.True);
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }


    [Test]
    public async Task Get_project_last_branch_build_test()
    {
        //Act
        var sut = await _apiManager.GetProjectLastBranchBuildAsync(TestCases.slug, TestCases.branch, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Get_project_last_build_test()
    {
        //Act
        var sut = await _apiManager.GetProjectLastBranchBuildAsync(TestCases.slug, branch: null, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Get_project_build_by_version_test()
    {
        //Act
        var sut = await _apiManager.GetProjectBuildByVersionAsync(TestCases.slug, TestCases.version, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Get_project_history_test()
    {
        //Act
        var sut = await _apiManager.GetProjectHistoryAsync(TestCases.slug, TestCases.branch, ct: _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Get_project_deployments_test()
    {
        //Act
        var sut = await _apiManager.GetProjectDeploymentsAsync(TestCases.slug, ct: _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Get_project_settings_test()
    {
        //Act
        var sut = await _apiManager.GetProjectSettingsAsync(TestCases.slug, ct: _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Get_project_settings_in_YAML_test()
    {
        //Act
        var sut = await _apiManager.GetProjectYamlSettingsAsync(TestCases.slug, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Get_project_environment_variables_test()
    {
        //Act
        var sut = await _apiManager.GetProjectEnvironmentAsync(TestCases.slug, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Post_AddProject_test()
    {
        //Act
        var sut = await _apiManager.AddProjectAsync(TestCases.repositoryProvider, TestCases.repositoryName, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task UpdateProjectEnvironmentVariables_using_json_body_from_file_test()
    {
        //Arrange
        var body = _updateProjectEnvironmentVariablesBody;
        var file = body.WriteToTempFile();
        //Act
        var sut = await _apiManager.UpdateProjectEnvironmentVariablesAsync(TestCases.slug, file, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task UpdateProjectEnvironmentVariables_using_env_list_test()
    {
        //Arrange
        EncryptedEnvironmentCollection body = new(new List<string>
        {
            "api_key:very-secret-key-encrypted:true",
            "var1:new-value"
        });
        //Act
        var sut = await _apiManager.UpdateProjectEnvironmentVariablesAsync(TestCases.slug, body, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task PutUpdateProjectSettingsInYaml_test()
    {
        //Arrange
        var yaml = """
                   version: 1.0.{build}
                   build:
                     project: MySolution.sln
                     verbosity: minimal
                     publish_wap: true
                   """;
        var slug2 = "cloudbuilder";
        var file = yaml.WriteToTempFile();
        //Act
        var sut = await _apiManager.UpdateProjectSettingsInYamlAsync(slug2, file, _ct);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Put_update_project_build_number_test()
    {
        //Act
        var sut = await _apiManager.UpdateProjectBuildNumberAsync(TestCases.slug, 35, _ct);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Delete_project_build_cache_test()
    {
        //Act
        var sut = await _apiManager.DeleteProjectBuildCacheAsync(TestCases.slug, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Delete_project_test()
    {
        //Act
        var sut = await _apiManager.DeleteProjectAsync(TestCases.slug, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Start_build_most_recent_commit_test()
    {
        //Act
        var sut = await _apiManager.StartBuildMostRecentAsync(TestCases.slug, TestCases.branch, TestCases.dict, ct: _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Start_build_of_specific_branch_commit_test()
    {
        //Act
        var sut = await _apiManager.StartBuildCommitAsync(TestCases.slug, TestCases.branch, TestCases.commitId, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Re_run_build_test()
    {
        //Act
        var sut = await _apiManager.ReRunBuildCommitAsync(TestCases.buildId, TestCases.reRunIncomplete, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Start_build_of_pull_request_test()
    {
        //Act
        var sut = await _apiManager.StartBuildPrAsync(TestCases.slug, TestCases.pullRequestId, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Cancel_build_test()
    {
        //Act
        var sut = await _apiManager.CancelBuildAsync(TestCases.slug, TestCases.version, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Delete_builds_test()
    {
        //Act
        var sut = await _apiManager.DeleteBuildsAsync(new[] { TestCases.buildId }, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    [Test]
    public async Task Download_build_log_test()
    {
        //Act
        var sut = await _apiManager.DownloadBuildLogAsync(TestCases.jobId, _ct);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(sut.IsSuccess, Is.True);
        });
    }

    private readonly string _updateProjectEnvironmentVariablesBody =
        """
              [
              {
                  "name":"api_key",
                  "value":{
                      "isEncrypted":true,
                      "value":"very-secret-key-encrypted"
                  }
              },
              {
                  "name":"var1",
                  "value":{
                      "isEncrypted":false,
                      "value":"new-value"
                  }
              }
              ]
            """;

}
