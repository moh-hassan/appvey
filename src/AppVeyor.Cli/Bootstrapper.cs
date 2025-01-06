// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli;

using Api;
using Api.Utility;
using Commands;
using Cli = DotMake.CommandLine.Cli;
#pragma warning disable 8618
public static class Bootstrapper
{
    public static ResponseResult ResponseResult { get; set; } = ResponseResult.Default();
    public static async Task<int> StartAsync(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        // Hook into Ctrl+C event
        Console.CancelKeyPress += (s, a) =>
        {
            a.Cancel = true; // Prevent default termination
            WriteWarning("Application is stopped by user.");
            cancellationTokenSource.Cancel(); // Cancel the token
        };

        var setting = new CliSettings
        {
            EnableDiagramDirective = true,
            ProcessTerminationTimeout = TimeSpan.FromSeconds(5.0),
            EnableEnvironmentVariablesDirective = true,
            Theme = CliTheme.Green            
        };
        string[] helpArgs = { "-h", "-?", "--help", "--version" };

        var pi = new AppVersionInfo(typeof(Program).Assembly);
        var heading = pi.Heading;
        WriteInfo(heading);
        var exitCode = await Cli.RunAsync<AppveyorCommand>(args, setting, cancellationTokenSource.Token);
        
        if (exitCode == 0)
        {
            if (!(args.Length == 0 || helpArgs.Any(args.Contains)))
                WriteSuccess("Exit code 0");
        }
        else
        {
            WriteVitalError($"Exit code: {exitCode}");
        }

        return exitCode;
    }
}
