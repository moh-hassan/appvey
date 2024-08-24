// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

// ReSharper disable All
namespace AppVeyor.Api;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal enum RepositoryProvider
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
