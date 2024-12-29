// Copyright (c) Mohamed Hassan & Contributors. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

using System.Text;
using Api;
using Api.Exceptions;
using Api.Security;

#nullable disable
[CliCommand(
    Description = "Configure Appveyor token and account and allow storing token to windows Credential Manager",
    Parent = typeof(AppveyorCommand))]
public class ConfigCommand
{
    [CliOption(Required = true, Description = "Appveyor User account")]
    public virtual string Account { get; set; }

    [CliOption(Required = false, Description = "Appveyor token v2")]
    public string Token { get; set; }

    //store token in credential manager
    [CliOption(Required = false, Name = "--use-cred", Aliases = ["--uc"],
        Description = "Store token in Windows Credential Manager (Windows only).")]
    public bool UseCredential { get; set; }

    [CliOption(Required = true, AllowedValues = ["save", "remove", "info"],
        Description = "show/save/remove configuration")]
    public string Action { get; set; }

    private IEnv Env => ServiceLocator.GetService<IEnv>();

    public async Task<int> RunAsync(CliContext context)
    {
        if (Env == null) throw new AppveyorException("Environment is not available.");

        switch (Action)
        {
            case "save":
                if (string.IsNullOrEmpty(Token))
                    throw new AppveyorException("Option '--token' is required.");
                if (UseCredential)
                {
                    StoreCred();
                }
                else
                {
                    Env.StoreToken(Token);
                    Env.StoreAccount(Account);
                }

                WriteLine("Configuration is saved.");
                return await Task.FromResult(0);

            case "remove":
                return await RemoveConfigurationAsync();

            case "info":
                var result = GetInfo();
                WriteLine(result.ToString());
                return await Task.FromResult(0);

            default:
                throw new AppveyorException("Invalid Configuration action.");
        }
    }

    private async Task<int> RemoveConfigurationAsync()
    {
        if (!Confirm())
        {
            WriteLine("Configuration is not removed.");
            return await Task.FromResult(0);
        }

        //remove configuration from environment

        if (!Env.IsExists(Account))
        {
            WriteLine($"Account '{Account}' is not stored in Environment.");
        }
        else
        {
            WriteInfo($"Removing account '{Account}' from Environment.");
            Env.RemoveAccount();
            WriteInfo("Removing token from Environment.");
            Env.RemoveToken();
        }

        //remove configuration from credential manager
        var cm = new WindowsCredentialManager();
        var result = cm.TryDeleteToken(Account);
        if (result)
            WriteInfo("Token is removed from Windows Credential Manager.");
        else
            WriteLine("Token is not found in Windows Credential Manager.");
        return await Task.FromResult(0);
    }

    private void StoreCred()
    {
        if (!UseCredential) return;
        var cm = new WindowsCredentialManager();
        var canStore = cm.TryStoreToken(Account, Token);

        WriteLine(canStore
            ? "Token is stored in Windows Credential Manager successfully."
            : "Token could not be stored in Windows Credential Manager.");
    }

    private StringBuilder GetInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Configuration Info:");

        if (Env.IsExists(Account))
        {
            sb.AppendLine($"Account '{Account}' is stored in Environment APPVEYOR_ACCOUNT");
            if (Env.GetToken() is { } _) sb.AppendLine("Token is stored in Environment.");
        }
        else
            sb.AppendLine($"Account '{Account}' isn't stored in Environment.");


        if (WindowsCredentialManager.IsExists(Account))
            sb.AppendLine("Token is stored in Windows Credential Manager.");
        else
            sb.AppendLine($"Token for account: '{Account}' isn't stored in Windows Credential Manager.");

        return sb;
    }
    private bool Confirm()
    {
        Console.Write($"Are you sure to remove account: '{Account}'? (y/n) ");
        var key = Console.Read();
        var answer = key is 'y' or 'Y';
        Console.WriteLine();
        return answer;
    }

}
#nullable restore
