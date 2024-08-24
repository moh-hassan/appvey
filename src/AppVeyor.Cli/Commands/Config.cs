// Copyright (c) Mohamed Hassan & Contributors. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

#nullable disable
[CliCommand(Description = "Configure Appveyor CI token and account",
    Parent = typeof(AppveyorCommand))]
public class Config
{
    [CliOption(Required = true, Description = "Appveyor token v2")]
    public string Token { get; set; }

    [CliOption(Required = true, Description = "Appveyor User account")]
    public virtual string Account { get; set; }

    public string ProxyAddress { get; set; }

    public string ProxyUser { get; set; }

    public async Task<int> RunAsync(CliContext context)
    {
        StoreEnv("APPVEYOR_TOKEN", Token);
        StoreEnv("APPVEYOR_ACCOUNT", Account);
        StoreEnv("HTTP_PROXY", ProxyAddress);
        StoreEnv("HTTP_PROXY_USER", ProxyUser);
        WriteLine("Configuration saved.");
        return await Task.FromResult(0);
    }

    private void StoreEnv(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
            Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.User);
    }
}
#nullable restore
