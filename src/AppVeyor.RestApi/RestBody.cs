// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using Collection;
using RestApi.Extensions;

public static class RestBody
{
    public static string BuildMostRecentBody(
        string accountName,
        string projectSlug,
        string branch,
        EnvironmentDictionary? environmentVariables)
    {
        environmentVariables ??= [];
        var body = new
        {
            accountName,
            projectSlug,
            branch,
            environmentVariables
        };
        return body.ToJson();
    }

    public static string BuildCommitBody(
        string accountName,
        string projectSlug,
        string branch,
        string commitId)
    {
        var body = new
        {
            accountName,
            projectSlug,
            branch,
            commitId
        };
        return body.ToJson();
    }

    public static string ReRunBuildBody(string buildId, bool reRunIncomplete)
    {
        var body = new
        {
            buildId,
            reRunIncomplete
        };
        return body.ToJson();
    }

    public static string PostStartBuildPrBody(string accountName, string projectSlug, string pullRequestId)
    {
        var body = new
        {
            accountName,
            projectSlug,
            pullRequestId
        };
        return body.ToJson();
    }

    public static string AddCollaboratorBody(string email, string roleId)
    {
        var body = new
        {
            email,
            roleId
        };
        return body.ToJson();
    }

    public static string PutCollaboratorsBody(string userId, string roleId)
    {
        var body = new
        {
            userId,
            roleId
        };
        return body.ToJson();
    }

    public static string PostProjectBody(string repositoryProvider, string repositoryName)
    {
        var body = new
        {
            repositoryProvider,
            repositoryName
        };
        return body.ToJson();
    }

    public static string UpdateProjectBuildNumber(int nextBuildNumber)
    {
        var body = new
        {
            nextBuildNumber
        };

        return body.ToJson();
    }
}

