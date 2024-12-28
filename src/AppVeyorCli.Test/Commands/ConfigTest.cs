// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test.Commands;
using System;
using System.Threading.Tasks;
using Api;
using Api.Exceptions;
using Api.Utility;
using AppVeyor.Api.Security;
using Extensions;
using FluentAssertions;

public class ConfigTest
{
    private bool _use_cred = false;
    private IEnv _env;

    [OneTimeSetUp]
    public void Setup()
    {
        //use dummy environment
        ServiceLocator.RegisterService<IEnv>(new DummyEnv());
        _env = ServiceLocator.GetService<IEnv>();
    }

    [SetUp]
    public void Setup2()
    {
        Logger.Clear();
    }

    [TearDown]
    public void TearDown()
    {
        _env.Clear();
    }

    [Test]
    public async Task A1_Token_Save_To_Environment()
    {
        // Arrange
        var args = "config --account testAccount --token testToken --action save".SplitArgs();

        // Act
        await Bootstrapper.StartAsync(args);

        // Assert
        _env.GetAccount().Should().Be("testAccount");
        _env.GetToken().Should().Be("testToken");
        Logger.Text.Should().Contain("Configuration is saved");
    }

    [Test]
    public async Task A1_Token_Save_To_WindowsCredentialManager()
    {
        // Arrange
        var args = "config --account testAccount --token testToken  --use-cred  --action save".SplitArgs();

        // Act
        await Bootstrapper.StartAsync(args);

        // Assert
        WindowsCredentialManager.IsExists("testAccount").Should().BeTrue();
        Logger.Text.Should().Contain("Configuration is saved");
        Logger.Text.Should().Contain("Token is stored in Windows Credential Manager successfully");
    }

    [Test]
    public void Throw_exception_when_save_and_token_is_missing()
    {
        // Arrange
        var args = "config --account testAccount --action save".SplitArgs();

        // Act & Assert
        var ex = Assert.ThrowsAsync<AppveyorException>(async () => await Bootstrapper.StartAsync(args));
        ex.Message.Should().Contain("Option '--token' is required.");
    }

    [Test]
    public async Task Display_configuration_info()
    {
        // Arrange
        var args = "config --account testAccount --action info".SplitArgs();

        // Act
        var result = await Bootstrapper.StartAsync(args);
        // Assert
        result.Should().Be(0);
        Logger.Text.Should().Contain("Configuration Info:");
    }
}
