// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

using System.IO;
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

    [CliOption(Required = false, Aliases = ["-t", "--token"],
        Description = "Appveyor token v2.\nTo enter your token, you have four options:\n" +
        "\u2022 Enter your token from your keyboard.\n" +
        "\u2022 Type a hyphen ('-'): This allows you to directly type or paste your token from your keyboard.\n" +
        "\u2022 Type '@' followed by the filename: This tells the system to read your token from the specified file.\n" +
        "\u2022 Ignore this option: If you don't enter anything, the system will try to automatically read your token from an environment variable or Windows Credential Manager: Available for Windows users only. This assumes you have already set up the token using config command.")]
    public string Token { get; set; }

    [CliOption(Required = false, Aliases = ["-a", "--account"],
        Description = "Appveyor User account: Optional. If not provided, the system will look for your account in the environment variables. This assumes you have already set up the account using config command.")]
    public virtual string Account { get; set; }

    [CliOption(Required = false, HelpName = "http://proxy:port",
        Aliases = ["-p", "--pa"],
        Description = "Proxy server should be in the form http://proxy:port",
        ValidationPattern = "^https?:\\/\\/[a-zA-Z0-9.-]+:[0-9]+$",
        ValidationMessage = "Proxy server should be in the form http://proxy:port")]
    public string ProxyAddress { get; set; }

    [CliOption(Required = false, Name = "-u", HelpName = "username:password",
        Aliases = ["-u", "--pu", "--proxy-user"],
        Description = "Proxy user/password should be in the form 'username:password'", ValidationPattern = "^[^:]+:[^:]+$",
        ValidationMessage = "Proxy user/password should be in the form 'username:password'")]
    public string ProxyUser { get; set; }

    [CliOption(Required = false, Aliases = ["--save"],
        Description = "File to save output response.")]
    public FileInfo Save { get; set; }

    [CliOption(Required = false, Aliases = ["-o", "--output"],
        Description = "File to save screen output.")]
    public FileInfo Output { get; set; }

    [CliOption(Required = false, Aliases = ["-w", "--wi", "--what-if"],
        Description = "Run the command without executing the actions of the command so no changes occur.\r\nIt displays optins and argument values and the expected Http Request:\r\n (Url, Method  <Get|Post|Put|Delete>, JsonBody.")]
    public bool WhatIf { get; set; }
    [CliOption(Description = "Verbose http connection details.",
        Name = "--verbose",
       Aliases = ["--verbose"]
        )]
    public bool Verbose { get; set; }

    private IEnv Env => ServiceLocator.GetService<IEnv>();
    private HttpConnection _httpConnection;

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
        _httpConnection = HttpConnection
            .Create(Env, Account, Token, ProxyAddress, ProxyUser, Verbose);
        //Process request
        var ct = context.CancellationToken;
        using var apiManager = new ApiManager(_httpConnection);
        var result = await RunApiAsync(apiManager, ct).ConfigureAwait(false);
        Bootstrapper.ResponseResult = result;

        //what-if mode
        if (WhatIf)
            return WhatIfDisplay(context, result);

        if (result == null) return 1;

        var exitCode = result.ShowResult();
        if (exitCode != 0) return exitCode;
        if (result.IsSuccess && result.StatusCode != HttpStatusCode.NoContent
                             && result.ResponseString.Length > 0)
        {
            WriteInfo("\nOutput response:");
        }

       // Request = result.HttpRequest;
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
        return Task.CompletedTask;
    }

    protected RestApi.Model.Build PrintBuildReport(ResponseResult result, bool browse, string slug)
    {
        var account= _httpConnection.AccountCredential.UserName;
        var buildReport = new BuildReporting(result, slug, account);
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

    protected virtual int WhatIfDisplay(CliContext context, ResponseResult result)
    {
        WriteInfo($"What-if mode.");
        WriteInfo($"{Title}");
        WriteInfo("CommandLine options:");
        context.ShowValues();
        WriteInfo($"Run Command: {Tag.Q()}");
        WriteLine();
        result.ShowResult();
        return 0;
    }
}
