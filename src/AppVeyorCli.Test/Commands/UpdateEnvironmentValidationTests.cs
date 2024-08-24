// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test;

using Cli.Commands.Project;

public class UpdateEnvironmentValidationTests
{
    [Test]
    public void IsValidOptions_BothNull_ReturnsFalse()
    {
        // Arrange
        var updateEnvironment = new UpdateEnvironment
        {
            Env = null,
            Json = null
        };
        // Act
        var result = updateEnvironment.IsValidOptions();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidOptions_BothNotNull_ReturnsFalse()
    {
        // Arrange
        var updateEnvironment = new UpdateEnvironment()
        {
            Env = ["name:value"],
            Json = new FileInfo("test.json")
        };

        // Act
        var result = updateEnvironment.IsValidOptions();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidOptions_OneNull_ReturnsTrue()
    {
        // Arrange
        var updateEnvironment = new UpdateEnvironment()
        {
            Env = ["name:value"],
        };

        // Act
        var result = updateEnvironment.IsValidOptions();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsValidOptions_OtherNull_ReturnsTrue()
    {
        // Arrange
        var updateEnvironment = new UpdateEnvironment()
        {
            Json = new FileInfo("test.json")
        };

        // Act
        var result = updateEnvironment.IsValidOptions();

        // Assert
        Assert.That(result, Is.True);
    }
}
