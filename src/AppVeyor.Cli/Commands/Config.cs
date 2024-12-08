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
public class Config
{
    [CliOption(Required = false, Description = "Appveyor User account")]
    public virtual string Account { get; set; }

    [CliOption(Required = false, Description = "Appveyor token v2")]
    public string Token { get; set; }

    //store token in credential manager
    [CliOption(Required = false, Name = "--use-cred", Aliases = ["--uc"],
        Description = "Store token in Windows Credential Manager (Windows only).")]
    public bool UseCredential { get; set; }

    [CliOption(Required = true, AllowedValues = ["save", "info"],
        Description = "show/save configuration")]
    public string Action { get; set; }

    public async Task<int> RunAsync(CliContext context)
    {
        var info = Action == "info";
        var save = Action == "save";

        if (info)
        {
            var result = GetInfo();
            WriteLine(result.ToString());
            return await Task.FromResult(0);
        }

        if (string.IsNullOrEmpty(Token))
        {
            throw new AppveyorException("Option '--token' is required.");
        }

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
    }

    private void StoreCred()
    {
        if (!UseCredential) return;
        var cm = new CredentialManager();
        var canStore = cm.TryStoreToken(Account, Token);

        WriteLine(canStore
            ? "Token is stored in Windows Credential Manager successfully."
            : "Token could not be stored in Windows Credential Manager.");
    }

    private StringBuilder GetInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Configuration Info:");

        if (Env.GetAccount() is { } account) sb.AppendLine($"Account '{account}' is stored in Environment APPVEYOR_ACCOUNT");

        if (Env.GetToken() is { } _) sb.AppendLine("Token is stored in Environment.");

        if (CredentialManager.IsExists(Account))
            sb.AppendLine("Token is stored in Windows Credential Manager.");
        else
            sb.AppendLine($"Token for account: '{Account}' isn't stored in Windows Credential Manager.");

        return sb;
    }
}
#nullable restore
