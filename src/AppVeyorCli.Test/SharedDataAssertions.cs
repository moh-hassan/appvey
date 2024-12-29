// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test;

using AppVeyor.Cli.Commands;
using FluentAssertions;

public static class SharedDataAssertions
{
    public static void ShouldBeEquivalentTo(string title, string tag, string request)
    {
        ExecutionInfo.Tag.Should().Be(tag, because: "Tags should match");
        ExecutionInfo.Title.Should().Be(title, because: "Titles should match");
        ExecutionInfo.Request.Should().Be(request, because: "Request should match");
    }
}
