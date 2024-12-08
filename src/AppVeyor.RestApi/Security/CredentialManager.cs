// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Security;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CredentialManagement;

internal class CredentialManager
{
    private static string SetTarget(string account)
    {
        if (!IsWindows())
            throw new AppveyorException("Credential Manager is allowed only for Windows.");
        _ = account ?? throw new ArgumentNullException(nameof(account));

        return $"Appveyor:account={account}";
    }

    public bool TryStoreToken(string account, string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentNullException(nameof(token));

        if (string.IsNullOrEmpty(account))
            throw new ArgumentNullException(nameof(account));

        using var cm = new CredentialManagement.Credential
        {
            Target = SetTarget(account),
            Username = account,
            Password = token,
            Type = CredentialType.Generic,
            PersistanceType = PersistanceType.LocalComputer,
            Description = "Token for the appveyor",
        };
        try
        {
            var result = cm.Save();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public bool TryRetrieveToken(string account, [NotNullWhen(true)] out string? token)
    {
        token = null;
        using var cm = new Credential
        {
            Target = SetTarget(account),
            Username = account,
        };
        try
        {
            var result = cm.Load();
            token = result ? cm.Password : null;
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public bool TryDeleteToken(string account)
    {
        using var cm = new Credential
        {
            Target = SetTarget(account),
            Username = account,
        };
        try
        {
            var isDeleted = cm.Delete();
            return isDeleted;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public static bool IsExists(string account)
    {
        var manager = new CredentialManager();
        return manager.TryRetrieveToken(account, out _);
    }

    public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
}
