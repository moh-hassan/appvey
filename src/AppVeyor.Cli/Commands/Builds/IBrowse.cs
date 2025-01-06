// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

public interface IBrowse
{
    [CliOption(Aliases = ["--browse"],
        Description = "Open the build page in the default browser.",
        Required = false)]
    bool Browse { get; set; }
}
