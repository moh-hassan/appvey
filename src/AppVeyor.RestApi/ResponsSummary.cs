// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyorCli;

using System.Text;
using AppVeyor.Api;
using AppVeyor.Api.Model;
using AppVeyor.RestApi.Extensions;

internal static class ResponseSummary
{
    //project build by version
    //project last build
    public static string ProjectBuildByVersion(this ResponseResult rr)
    {
        if (!rr.IsSuccess) return "";

        var json = rr.ResponseString;
        var model = json.ToObject<BuildInfo>();
        if (model == null)
        {
            return "Error: Could not deserialize json to ProjectHistory";
        }

        var sb = new StringBuilder();
        sb.AppendLine($"Project Slug: {model.Project.Slug}");
        sb.AppendLine($"Branch: {model.Project.RepositoryBranch}");
        sb.AppendLine($"BuildId: {model.Build.BuildId}");
        sb.AppendLine($"Version: {model.Build.Version}");
        sb.AppendLine($"JobId: {model.Build.Jobs[0].JobId}");
        sb.AppendLine($"CommitId: {model.Build.CommitId}");
        sb.AppendLine($"IsTag: {model.Build.IsTag}");
        sb.AppendLine($"Status: {model.Build.Status}");
        sb.AppendLine($"Created: {model.Build.Created}");
        return sb.ToString();
    }
}


