// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;
using Api.Model.AppveyorDeployment;
using ConsoleTables;
using RestApi.Extensions;

[CliCommand(Description = "Get project deployments",
    Parent = typeof(AppveyorCommand.ProjectCommand))]
public class Deploy : AppveyorCommandBase
{
    protected override string Title => "Project deployments ...";
    protected override string Tag => "project deploy";

    [CliOption(Description = "Project slug")]
    public string Slug { get; set; }

    [CliOption(Description = "Number of records", Name = "--records")]
    public int RecordsNumber { get; set; } = 20;

    [CliArgument(Description = "Start Deployment Id", Required = false)]
    public string StartDeploymentId { get; set; }

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        var result = await apiManager
            .GetProjectDeploymentsAsync(Slug, StartDeploymentId, RecordsNumber, ct)
            .ConfigureAwait(false);
        return result;
    }

    protected override void DisplayResponseResult(ResponseResult result)
    {
        var deployments = result.ResponseString.ToObject<DeplymentRoot>().deployments;
        if (!deployments.Any()) return;

        var table = new ConsoleTable("deploymentId", "status", "created");
        foreach (var deployment in deployments)
        {
            table.AddRow(deployment.deployment.deploymentId, deployment.deployment.status, deployment.deployment.created);
        }

        WriteLine(table.ToString());
        Console.WriteLine();
    }
}
#nullable restore

