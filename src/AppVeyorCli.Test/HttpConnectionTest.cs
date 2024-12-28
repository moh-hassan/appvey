// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

using AppVeyor.Api;
using AppVeyor.Api.Exceptions;

namespace AppVeyor.Test;

public class HttpConnectionTest
{
    private IEnv _env;
    private readonly string _account = "testAccount";
    private readonly string _token = "testToken";

    [SetUp]
    public void Setup()
    {
        _env = new DummyEnv();
    }

    [TearDown]
    public void TearDown()
    {
        _env.Clear();
    }

    [Test]
    public void Create_WithValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var proxyAddress = "http://proxy.test";
        var proxyUser = "user:password";
        var verbose = true;

        // Act
        var connection = HttpConnection
            .Create(_env, _account, _token, proxyAddress, proxyUser, verbose);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(connection.AccountCredential.UserName, Is.EqualTo(_account));
            Assert.That(connection.AccountCredential.Password, Is.EqualTo(_token));
            Assert.That(connection.ProxyAddress, Is.EqualTo(proxyAddress));
            Assert.That(connection.ProxyCredential.UserName, Is.EqualTo("user"));
            Assert.That(connection.ProxyCredential.Password, Is.EqualTo("password"));
            Assert.That(connection.Verbose, Is.True);
        });
    }


    [Test]
    public void Non_empty_account_token_ShouldInitializeCorrectly()
    {
        // Arrange
        // Act
        var connection = HttpConnection.Create(_env, _account, _token);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(connection.AccountCredential.UserName, Is.EqualTo(_account));
            Assert.That(connection.AccountCredential.Password, Is.EqualTo(_token));
        });
    }

    [Test]
    public void Empty_account_token_but_stored_in_environment_shouldinitializecorrectly()
    {
        // Arrange
        _env.StoreAccount(_account,_token);

        // Act
        var connection = HttpConnection.Create(_env);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(connection.AccountCredential.UserName, Is.EqualTo(_account));
            Assert.That(connection.AccountCredential.Password, Is.EqualTo(_token));
        });
    }

    [Test]
    public void Empty_account_token_and_not_stored_in_environment_should_throw_exception()
    {
        var ex = Assert.Throws<AppveyorException>(() => HttpConnection.Create(_env));
        Assert.That(ex.Message, Is.EqualTo("Account Exception: Account is null or empty."));
    }

    [Test]
    public void Valid_proxy_user_password_should_return_proxycredential()
    {
        // Arrange
        var userPassword = "user:password";

        // Act
        var connection = HttpConnection.Create(_env, _account, _token, proxyUser: userPassword);
        var credential = connection.ProxyCredential;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(credential.UserName, Is.EqualTo("user"));
            Assert.That(credential.Password, Is.EqualTo("password"));
        });
    }

    [Test]
    public void Invalid_user_password_should_return_networkcredential()
    {
        // Arrange
        var userPassword = "user";

        // Act
        var credential = HttpConnection.Create(_env, _account, _token, proxyUser: userPassword)
            .ProxyCredential;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(credential.UserName, Is.EqualTo("user"));
            Assert.That(credential.Password, Is.EqualTo(""));
        });
    }

    [Test]
    public void Empty_user_password_should_return_networkcredential()
    {
        // Arrange
        var userPassword = "";

        // Act
        var credential = HttpConnection.Create(_env, _account, _token, proxyUser: userPassword)
            .ProxyCredential;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(credential.UserName, Is.EqualTo(""));
            Assert.That(credential.Password, Is.EqualTo(""));
        });
    }
}
