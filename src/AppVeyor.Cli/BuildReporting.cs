// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli;

using System.Diagnostics;
using Api;
using RestApi.Extensions;
using RestApi.Model;

internal class BuildReporting
{
    private Build? _build;
    private string _slug;
    private string _account;

    public BuildReporting(ResponseResult rr, string slug, string account)
    {
        _slug = slug;
        _account = account;
        _build = rr.IsSuccess ? rr.ResponseString.ToObject<Build>() : default;
    }

    public Build? PrintReport(bool browse)
    {
        _ = ReportBuildCancel();
        var page = GetBuildPage();
        if (browse) BrowsePage(page);
        return _build;
    }

    private string GetBuildPage()
    {
        var buildId = _build?.BuildId;
        if (buildId == null) return string.Empty;
        var buildPage = ApiEndpoints.GetBuildPageUrl(_account, _slug, buildId);
        WriteLine("You can browse Build Page at url:");
        WriteInfo(buildPage);
        return buildPage;
    }

    private string ReportBuildCancel()
    {
        var version = _build?.Version;
        if (version == null) return "";
        var cancelCommand = $"build cancel --slug {_slug} {_build?.Version}";
        WriteLine($"To cancel build:> {cancelCommand}");
        return cancelCommand;
    }

    private void BrowsePage(string url)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true,
        };
        Process.Start(processInfo);
    }
}
