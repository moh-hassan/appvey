// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Utility;

using System.Reflection;
using Api;

internal class AppVersionInfo
{
    private Assembly Assembly { get; }
    private AssemblyName AssemblyName { get; }
    public string ExecutableName { get; }
    public string Product { get; } = "appvey";
    public string Version { get; } = "1.0.0";
    public string VersionWithShortCommit { get; }
    public string VersionWithNoCommit { get; }
    public string Copyright { get; }
    public string Heading { get; }

    public AppVersionInfo() : this(typeof(ApiClient).Assembly)
    {
    }

    public AppVersionInfo(Assembly assembly)
    {
        Assembly = assembly;
        AssemblyName = Assembly.GetName();
        ExecutableName = AssemblyName.Name ?? "appvey";

        var product = Assembly.GetCustomAttribute<AssemblyProductAttribute>();
        Product = product?.Product ?? "appvey";

        var informationalVersion = Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (informationalVersion != null)
            Version = informationalVersion.InformationalVersion.TrimStart('v') ?? "1.0.0";
        VersionWithShortCommit = Version;
        VersionWithNoCommit = GetVersionWithCommit(Version, 0);
        var assemblyCopyright = Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
        Copyright = (assemblyCopyright != null) ? assemblyCopyright.Copyright : string.Empty;

        Heading = $"{ExecutableName} {VersionWithShortCommit}{Environment.NewLine}";
    }

    private string GetVersionWithCommit(string fullVersion, int n = 7)
    {
        var pairs = fullVersion.Split('+');
        if (pairs.Length == 1)
            return fullVersion;

        var v = pairs[0];
        var h = pairs[1];
        return n == 0 ? v : $"{v}+{h[..n]}";
    }
}

