// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using System.Threading;
using Api;
using ConsoleTables;
using RestApi.Extensions;
using RestApi.Model;

[CliCommand(Description = "Get project history", Parent = typeof(AppveyorCommand.ProjectCommand))]
public class History : AppveyorCommandBase
{
    protected override string Title => "Get project history ...";
    protected override string Tag => "project history";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliOption(Required = false)]
    public string Branch { get; set; }

    [CliOption(Description = "Number of records", Name = "--records")]
    public int RecordsNumber { get; set; } = 20;

    [CliOption(Description = "Start from build id", Required = false, Name = "--build-id")]
    public string StartBuildId { get; set; }

    [CliOption(Description = "Delete both failed and cancelled builds.", Required = false)]
    public bool Delete { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        var result = await apiManager
            .GetProjectHistoryAsync(Slug, Branch, RecordsNumber, StartBuildId, ct)
            .ConfigureAwait(false);
        return result;
    }

    private async Task DeleteFailedAsync(ApiManager apiManager, ResponseResult result, CancellationToken ct)
    {
        var confirm = Confirm("Delete both failed and cancelled builds.");
        if (!confirm) return;
        var buildIds = FilterBuildIds(result);
        _ = await apiManager.DeleteBuildsAsync(buildIds, ct).ConfigureAwait(false);
    }

    private List<string> FilterBuildIds(ResponseResult result)
    {
        var filter = new[] { "failed", "cancelled" };
        var buildIds = result.ResponseString.ToObject<ProjectHistory>()
            .Builds
            .Where(a => filter.Contains(a.Status))
            .Select(a => a.BuildId).Select(a => a.ToString())
            .Reverse()
            .ToList();
        return buildIds;
    }

    protected override void DisplayResponseResult(ResponseResult result)
    {
        WriteLine($"Display first {RecordsNumber} records:");
        PrintHistory(result.ResponseString);
    }

    private void PrintHistory(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            WriteLine("No data found");
            return;
        }

        var model = json.ToObject<ProjectHistory>();
        if (model == null)
        {
            WriteLine("Error: Could not deserialize json to ProjectHistory");
            WriteLine(json);
            return;
        }

        var builds = model.Builds;
        var table = new ConsoleTable("Version", "BuildId", "CommitId", "Status", "Created");
        foreach (var build in builds)
        {
            table.AddRow(build.Version, build.BuildId, build.CommitId[..9], build.Status, build.Created);
        }

        WriteLine(table.ToMinimalString());
    }

    protected override async Task PostCommandAsync(
        ApiManager apiManager,
        ResponseResult result,
        CancellationToken ct)
    {
        await base.PostCommandAsync(apiManager, result, ct);
        if (Delete)
            await DeleteFailedAsync(apiManager, result, ct);
    }
}

#nullable restore
