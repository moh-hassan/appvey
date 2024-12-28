// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Security;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Meziantou.Framework.Win32;

internal class WindowsCredentialManager
{
    internal string CreateTargetName(string account)
    {
        if (!IsWindows())
            throw new AppveyorException("Credential Manager is allowed only for Windows.");
        if (string.IsNullOrEmpty(account))
            throw new ArgumentNullException(nameof(account));
        return $"Appvey:account={account}";
    }

    public bool TryStoreToken(string account, string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentNullException(nameof(token));
        var target = CreateTargetName(account);
        CredentialManager.WriteCredential(target, account, token, CredentialPersistence.LocalMachine);
        return true;            
    }

    public bool TryReadToken(string account, [NotNullWhen(true)] out string? token)
    {
        token = null;
        var target = CreateTargetName(account);
        var cred = CredentialManager.ReadCredential(target, CredentialType.Generic);
        if (cred == null)
            return false;

        token = cred.Password ?? throw new InvalidOperationException("Credential password is null.");
        return true;
    }

    public void DeleteToken(string account)
    {
        var target = CreateTargetName(account);
        CredentialManager.DeleteCredential(target, CredentialType.Generic);
    }

    public static bool IsExists(string account)
    {
        var manager = new WindowsCredentialManager();
        return manager.TryReadToken(account, out _);
    }

    public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
}
