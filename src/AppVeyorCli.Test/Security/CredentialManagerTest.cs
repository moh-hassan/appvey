// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test.Security;
using System;
using AppVeyor.Api.Security;

public class CredentialManagerTest
{
    [Test]
    public void TryStoreToken_ValidAccountAndToken_ReturnsTrue()
    {
        var cm = new CredentialManager();
        var account = "test-account";
        var token = "test-token";
        var result = cm.TryStoreToken(account, token);
        Assert.That(result, Is.True);
    }

    [Test]
    public void TryStoreToken_EmptyToken_ThrowsArgumentNullException()
    {
        var cm = new CredentialManager();
        var account = "test-account";
        Assert.Throws<ArgumentNullException>(() => cm.TryStoreToken(account, string.Empty));
    }

    [Test]
    public void TryStoreToken_EmptyAccount_ThrowsArgumentNullException()
    {
        var cm = new CredentialManager();
        var token = "test-token";
        Assert.Throws<ArgumentNullException>(() => cm.TryStoreToken(string.Empty, token));
    }

    [Test]
    public void TryRetrieveToken_ValidAccount_ReturnsTrueAndToken()
    {
        var cm = new CredentialManager();
        var account = "test-account";
        var token = "test-token";
        cm.TryStoreToken(account, token);
        var result = cm.TryRetrieveToken(account, out var retrievedToken);
        Assert.That(result, Is.True);
        Assert.That(retrievedToken, Is.EqualTo(token));
    }

    [Test]
    public void TryRetrieveToken_InvalidAccount_ReturnsFalse()
    {
        var cm = new CredentialManager();
        var account = "invalid-account";
        var result = cm.TryRetrieveToken(account, out var retrievedToken);
        Assert.That(result, Is.False);
        Assert.That(retrievedToken, Is.Null);
    }

    [Test]
    public void IsExists_ValidAccount_ReturnsTrue()
    {
        var cm = new CredentialManager();
        var account = "test-account";
        var token = "test-token";
        cm.TryStoreToken(account, token);
        var result = CredentialManager .IsExists(account);
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsExists_InvalidAccount_ReturnsFalse()
    {
        var cm = new CredentialManager();
        var account = "invalid-account";
        var result = CredentialManager.IsExists(account);
        Assert.That(result, Is.False);
    }

    [Test]
    public void TryDeleteToken_ValidAccount_ReturnsTrue()
    {
        var cm = new CredentialManager();
        var account = "test-account";
        var token = "test-token";
        cm.TryStoreToken(account, token);
        var result = cm.TryDeleteToken(account);
        Assert.That(result, Is.True);
    }

    [Test]
    public void TryDeleteToken_InvalidAccount_ReturnsFalse()
    {
        var cm = new CredentialManager();
        var account = "invalid-account";
        var result = cm.TryDeleteToken(account);
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase("test-account", "test-account")]
    [TestCase("test-account2", "test-account2")]
    public void Store_multi_account(string account, string token)
    {
        var cm = new CredentialManager();
        var result = cm.TryStoreToken(account, token);
        Assert.That(result, Is.True);
    }

    //------------------
    //  [Test]
    public void Test1()
    {
        var cm = new CredentialManager();
        var userName = "moh-hassan";
        var password = Environment.GetEnvironmentVariable(userName) ?? "unknown";
        var canStore = cm.TryStoreToken(userName, password);
        var canReterive = cm.TryRetrieveToken(userName, out var password2);
        // var canDelete = cm.TryDeleteToken(userName);
        Assert.That(canStore, Is.True);
        Assert.That(password2, Is.EqualTo(password));
        Assert.That(canReterive, Is.True);
    }
    
    //[Test]
    //[TestCase("moh-hassan", "xyz")]
    //[TestCase("moh-hassan2", "abc")]
    public void Test1b(string account, string token)
    {
        var cm = new CredentialManager();
        var userName = account;
        //string password = Environment.GetEnvironmentVariable(userName) ?? "unknown";
        var canStore = cm.TryStoreToken(userName, token);
        var canReterive = cm.TryRetrieveToken(userName, out var password2);
        // var canDelete = cm.TryDeleteToken(userName);
        Assert.That(canStore, Is.True);
        Assert.That(password2, Is.EqualTo(token));
        Assert.That(canReterive, Is.True);
    }
    //[Test]
    //public void Test1A()
    //{
    //    var cm = new CredentialManager();
    //    string userName = "APPVEYOR_TOKEN";
    //    string password = Environment.GetEnvironmentVariable(userName) ?? "unknown";
    //    var canStore = cm.TryStoreToken(password);
    //    var canReterive = cm.TryRetrieveToken( out var password2);
    //    // var canDelete = cm.TryDeleteToken(userName);
    //    Assert.That(canStore, Is.True);
    //    Assert.That(password2, Is.EqualTo(password));
    //    Assert.That(canReterive, Is.True);
    //}

   // [Test]
    public void Test2()
    {
        var cm = new CredentialManager();
        var userName = "APPVEYOR_TOKEN";
        var isDeleted = cm.TryDeleteToken(userName);
        Assert.That(isDeleted, Is.True);
    }
}
