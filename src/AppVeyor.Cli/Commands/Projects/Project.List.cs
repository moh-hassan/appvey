// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;

#nullable disable
using Api;
using ConsoleTables;
using RestApi.Extensions;
using RestApi.Model;

[CliCommand(Description = "List all projects", Name = "list",
    Parent = typeof(AppveyorCommand.ProjectCommand))]
public class ListCommand : AppveyorCommandBase
{
    protected override string Title => "Get projects ...";
    protected override string Tag => "project list";

    protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        if (apiManager is null) throw new ArgumentNullException(nameof(apiManager));
        var result = await apiManager.GetProjectsAsync(ct).ConfigureAwait(false);
        return result;
    }

    protected override void DisplayResponseResult(ResponseResult result)
    {
        var model = result.ResponseString.ToObject<List<Project>>()?
            .Select(a => new { a.ProjectId, a.Name, a.Created });
        var iEnumerable = model?.ToList();
        if (iEnumerable != null && iEnumerable.Any())
        {
            var table = new ConsoleTable("ProjectId", "Name", "Created");
            foreach (var item in iEnumerable)
            {
                table.AddRow(item.ProjectId, item.Name, item.Created);
            }

            WriteLine(table.ToMinimalString());
        }
    }
}

#nullable restore
