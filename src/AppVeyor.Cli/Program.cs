// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

namespace AppVeyor.Cli;

using Api;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            ServiceLocator.RegisterService<IEnv>(new Env());
            var result= await Bootstrapper
                .StartAsync(args)
                .ConfigureAwait(false);
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
