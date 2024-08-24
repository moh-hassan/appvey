// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test;

#pragma warning disable 
using Api.Collection;
using Api.Model;
using Api.Utility;
using RestApi.Extensions;

#nullable disable
internal static class TestCases
{
    public static string develop;
    public static string token;
    public static string accountName;
    public static string slug;
    public static string branch;
    public static string repositoryProvider = "gitHub";
    public static string repositoryName;
    public static string version;
    public static string commitId;
    public static string buildId;
    public static bool reRunIncomplete;
    public static string pullRequestId;
    public static string jobId;
    public static string buildNumber;

    public static EnvironmentDictionary dict = [];

    static TestCases()
    {
        develop = Environment.GetEnvironmentVariable("DEVELOP");
        SetUpTestCases(develop);
    }

    public static void SetUpTestCases(string developer)
    {
        develop = developer;
        if (developer == "1") // develop
        {
            token = "secret";
            accountName = "moh-hassan";
            slug = "cloudbuilder";
            branch = "master";
            repositoryProvider = "gitHub";
            repositoryName = "test";
            commitId = "3e9d9468";
            version = "1.0.0-dev-1";
            buildId = "50127590";
            jobId = "3akc42kgf8d7fvyt";
            reRunIncomplete = false;
            pullRequestId = "123";
            buildNumber = "35";
            dict = new(["api_key:very-secret-key-encrypted", "var1:new-value"]);

            //setup env vars
            Environment.SetEnvironmentVariable("APPVEYOR_TOKEN", token);
            Environment.SetEnvironmentVariable("APPVEYOR_ACCOUNT", accountName);
        }
        else // production, default (develop == "0")
        {
            token = null;
            accountName = null;
            slug = "cloudbuilder";
            branch = "master";
            repositoryProvider = "gitHub";
            repositoryName = "test";
            commitId = "e690e763";
            version = "1.0.0-dev-1";
            buildId = "50365708";
            jobId = "a44ttckf4cb6mw0i";
            reRunIncomplete = false;
            pullRequestId = "123";

            dict = new(["api_key:very-secret-key-encrypted", "var1:new-value"]);
        }
    }

    public static string SetupProduction()
    {
        develop = "0";
        Environment.SetEnvironmentVariable("DEVELOP", develop);
        SetUpTestCases(develop);
        ColorWriter.SetConsole(new ConsoleWrapper());
        return develop;
    }

    public static string SetupTestServer()
    {
        develop = "1";
        Environment.SetEnvironmentVariable("DEVELOP", develop);
        SetUpTestCases(develop);
        ColorWriter.SetConsole(new DummyConsole());
        return develop;
    }

    public static void SetupBuildInfo(string json)
    {
        var model = json.ToObject<BuildInfo>();
        if (model == null) return;
        slug = model.Project.Slug;
        branch = model.Project.RepositoryBranch;
        buildId = model.Build.BuildId.ToString();
        version = model.Build.Version;
        jobId = model.Build.Jobs[0].JobId;
        commitId = model.Build.CommitId;
        buildNumber = (model.Build.BuildNumber + 1).ToString();
    }
}
#nullable restore
