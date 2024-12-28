// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

public enum TokenStorage
{
    None = 0,
    EnvironmentVariable,
    WindowsCredentialManager
}
