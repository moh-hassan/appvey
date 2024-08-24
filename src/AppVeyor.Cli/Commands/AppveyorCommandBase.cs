// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

using System.Net;
using System.Threading;
using Api;
using Api.Utility;
using AppVeyor.Api.Exceptions;
using AppVeyor.Cli.Commands.Project;
using RestApi.Extensions;

#nullable disable
public abstract class AppveyorCommandBase
{
    protected virtual string Title { get; }
    protected virtual string Tag { get; }
    protected virtual string Request { get; set; }

    [CliOption(Description = "Verbose http connection.")]
    public bool Verbose { get; set; }

    [CliOption(Required = false,
        Description = "Appveyor token v2. Type dash '-' to enter/paste token from keyboard or @filename to read token from filename or skip it to read token from environment var.")
    ]
    public string Token { get; set; }

    [CliOption(Required = false,
        Description = "Appveyor User account or skip it to read account from environment var.")]
    public virtual string Account { get; set; }

    [CliOption(Required = false, Description = "Proxy server should be in the form http://proxy:port", ValidationPattern = "^https?:\\/\\/[a-zA-Z0-9.-]+:[0-9]+$",
        ValidationMessage = "Proxy server should be in the form http://proxy:port")]
    public string ProxyAddress { get; set; }

    [CliOption(Required = false, Aliases = ["-u"],
        Description = "Proxy user/password should be in the form username:password", ValidationPattern = "^[^:]+:[^:]+$",
        ValidationMessage = "Proxy user/password should be in the form username:password")]
    public string ProxyUser { get; set; }

    [CliOption(Required = false, Description = "File to save output response.")]
    public FileInfo Save { get; set; }

    [CliOption(Required = false, Description = "File to save screen output.")]
    public FileInfo Output { get; set; }

    protected virtual ExecutionInfo ExecutionInfoCollector { get; set; }

    protected ApiManager GetApiManager()
    {
        var httpConnection = new HttpConnection(Account, Token)
        {
            ProxyAddress = ProxyAddress,
            ProxyUser = ProxyUser,
            Verbose = Verbose
        };
        return new(httpConnection);
    }

    protected virtual Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    protected virtual void DisplayResponseResult(ResponseResult result)
    {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (!result.IsSuccess) return;
        WriteLine(result.ResponseString.JsonFormat());
    }

    public virtual async Task<int> RunAsync(CliContext context)
    {
        Token = InputHelper.ProcessToken(Token);
        var ct = context.CancellationToken;
        using var apiManager = GetApiManager();
        var result = await RunApiAsync(apiManager, ct).ConfigureAwait(false);

        if (result == null) return 1;

        var exitCode = result.ShowResult();
        if (exitCode != 0) return exitCode;
        if (result.IsSuccess && result.StatusCode != HttpStatusCode.NoContent
                             && result.ResponseString.Length > 0)
        {
            WriteInfo("\nOutput response:");
        }

        DisplayResponseResult(result);
        if (Save != null)
            result.SaveResponse(Save);
        if (Output != null)
            Logger.Save(Output);
        await PostCommandAsync(apiManager, result, ct);
        return 0;
    }

    protected virtual Task PostCommandAsync(ApiManager apiManager, ResponseResult result, CancellationToken ct)
    {
        ExecutionInfoCollector = new(Title, Tag)
        {
            Request = result.Request,
        };
        ExecutionInfo.Copy(ExecutionInfoCollector, Program.ExecutionInfo);
#if DEBUG
        ExecutionInfoCollector.Show(Verbose);
#endif
        return Task.CompletedTask;
    }

    protected RestApi.Model.Build PrintBuildReport(ResponseResult result, bool browse, string slug)
    {
        var buildReport = new BuildReporting(result, slug, Account);
        var build = buildReport.PrintReport(browse);
        return build;
    }

    protected bool Confirm(string message)
    {
        Console.Write(message);
        Console.WriteLine(" Are you sure? (y/n)");
        var key = Console.Read();
        var answer = key is 'y' or 'Y';
        Console.WriteLine();
        return answer;
    }

    protected void ValidateOptions(string buildVersion, string jobId)
    {
        if (!string.IsNullOrEmpty(jobId) && !string.IsNullOrEmpty(buildVersion))
        {
            WriteWarning("Warning: Both options job-id and build-version are mutually exclusive. Ignoring 'job-id'");
            return;
        }

        if (string.IsNullOrEmpty(jobId) && string.IsNullOrEmpty(buildVersion))
        {
            throw new AppveyorException("Either job-id or build-version should be specified");
        }
    }
}
