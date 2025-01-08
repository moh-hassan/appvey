// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands.Project;
#nullable disable
#nullable restore

public enum RepositoryProvider
{
    gitHub,
    bitBucket,
    vso, //(Visual Studio Online)
    gitLab,
    kiln,
    stash,
    git,
    mercurial,
    subversion
}
