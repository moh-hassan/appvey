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

//*********To run this test, set _use_cred = true *************
public class ConfigTest
{
    private Bootstrapper bootstrapper;
    private string? _account;
    private string? _token;
    private bool _use_cred = false;

    [OneTimeSetUp]
    public void Setup()
    {
        if (!_use_cred) Assert.Ignore("Ignored.Set _use_cred =true");

        bootstrapper = new Bootstrapper();
        //store original environment if exists
        _account = Env.GetAccount();
        _token = Env.GetToken();
    }

    [SetUp]
    public void Setup2()
    {
        Logger.Clear();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        //restore original environment
        Env.StoreAccount(_account);
        Env.StoreToken(_token);
    }

    [Test]
    public async Task Should_Save_Configuration_To_Environment()
    {
        // Arrange
        var args = "config --account testAccount --token testToken --action save".SplitArgs();

        // Act
        await bootstrapper.StartAsync(args);

        // Assert
        Logger.Print();
        Env.GetAccount().Should().Be("testAccount");
        Env.GetToken().Should().Be("testToken");
    }

    [Test]
    public void Throw_exception_when_save_and_token_is_missing()
    {
        // Arrange
        var args = "config --account testAccount --action save".SplitArgs();

        // Act & Assert
        var ex = Assert.ThrowsAsync<AppveyorException>(async () => await bootstrapper.StartAsync(args));
        ex.Message.Should().Contain("Option '--token' is required.");
    }

    [Test]
    public async Task Display_configuration_info()
    {
        // Arrange
        var args = "config --account testAccount --action info".SplitArgs();

        // Act
        var result = await bootstrapper.StartAsync(args);
        Console.WriteLine($"++log: [{Logger.Text}]");
        // Assert
        result.Should().Be(0);
        Logger.Text.Should().Contain("Configuration Info:");
    }

    [Test]
    public async Task Store_token_in_credential_manager()
    {
        // Arrange
        var args = $"config --account testAccount --token testToken --action save --use-cred".SplitArgs();

        // Act
        await bootstrapper.StartAsync(args);

        // Assert
        CredentialManager.IsExists("testAccount").Should().BeTrue();
    }
}
