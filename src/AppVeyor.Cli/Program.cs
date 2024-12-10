// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

namespace AppVeyor.Cli;

using Commands;

public static class Program
{
    public static ExecutionInfo? ExecutionInfo { get; set; } = new();

    public static async Task<int> Main(string[] args)
    {
        try
        {
            var bootstrapper = new Bootstrapper();
            var result= await bootstrapper.StartAsync(args);
            return result;
        }
        catch (OperationCanceledException)
        {
            //do nothing
            Console.WriteLine("Operation is cancelled by user");
            return 0;
        }
        catch (Exception e)
        {
            WriteError($"Error: {e.Message}\n{e.InnerException?.Message}");
            WriteVitalError($"Exit code: 3");

#if DEBUG
            Console.WriteLine("---------------- Debug StackTrace------------------");
            Console.WriteLine(e.StackTrace);
#endif
            return 3;
        }
    }
}
