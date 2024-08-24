// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

namespace AppVeyor.Cli;

using System.Threading;
using Api.Utility;
using Commands;
using DotMake.CommandLine;

public static class Program
{
    public static ExecutionInfo ExecutionInfo { get; set; } = new();

    public static async Task<int> Main(string[] args)
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
            Theme = CliTheme.Green,
        };
        string[] helpArgs = { "-h", "-?", "--help", "--version" };
        try
        {
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
        catch (OperationCanceledException)
        {
            //do nothing
        }
        catch (Exception e)
        {
            WriteError($"Error: {e.Message}\n{e.InnerException?.Message}");
            WriteVitalError($"Exit code: 3");
#if DEBUG
            Console.WriteLine("---------------- Debug StackTrace------------------");
            Console.WriteLine(e.StackTrace);
#endif
        }

        return 1;
    }
}
