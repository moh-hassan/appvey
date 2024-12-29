// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test.Security;
using System;
using AppVeyor.Api.Security;

[NonParallelizable]
public class WindowsCredentialManagerTest
{
    //To run this test in Windows Environment only, set _canRun=true
    bool _canRun = false;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _canRun = _canRun && WindowsCredentialManager.IsWindows();
        if (!_canRun)
            Assert.Ignore("This CredentialManagerTest is ignored because _canRun=false.");
    }
   
    [Test]
    public void A00_get_targetName_should_be_correct()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        // Act
        var target = cm.CreateTargetName("test-account");
        // Assert
        Assert.That(target, Is.EqualTo("Appvey:account=test-account"));
    }

    [Test]
    public void A01_TryStoreToken_ValidAccountAndToken_ReturnsTrue()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        var account = "test-account";
        var token = "test-token";
        // Act
        var result = cm.TryStoreToken(account, token);
        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void A02_TryStoreToken_EmptyToken_ThrowsArgumentNullException()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        var account = "test-account";
        // Act
        var result = Assert.Throws<ArgumentNullException>(() => cm.TryStoreToken(account, string.Empty));
        // Assert
        Assert.That(result.Message, Is.EqualTo("Value cannot be null. (Parameter 'token')"));
    }

    [Test]
    public void A03a_TryStoreToken_EmptyAccount_ThrowsArgumentNullException()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        var token = "test-token";
        // Act, Assert
        var result = Assert.Throws<ArgumentNullException>(() => cm.TryStoreToken(string.Empty, token));
        Assert.That(result.Message, Is.EqualTo("Value cannot be null. (Parameter 'account')"));
    }

    [Test]
    public void A03b_TryStoreToken_EmptyToken_ThrowsArgumentNullException()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        var account = "test-token";
        // Act  , Assert
        Assert.Throws<ArgumentNullException>(() => cm.TryStoreToken(account, string.Empty));
    }

    [Test]
    public void A04_TryRetrieveToken_ValidAccount_ReturnsTrueAndToken()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        var account = "test-account";
        var token = "test-token";
        // Act
        cm.TryStoreToken(account, token);
        var result = cm.TryReadToken(account, out var retrievedToken);
        // Assert
        Assert.That(result, Is.True);
        Assert.That(retrievedToken, Is.EqualTo(token));
    }

    [Test]
    public void A05_TryRetrieveToken_InvalidAccount_ReturnsFalse()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        var account = "invalid-account";
        // Act
        var result = cm.TryReadToken(account, out var retrievedToken);
        // Assert
        Assert.That(result, Is.False);
        Assert.That(retrievedToken, Is.Null);
    }

    [Test]
    public void A06_IsExists_ValidAccount_ReturnsTrue()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        var account = "test-account";
        var token = "test-token";
        // Act
        cm.TryStoreToken(account, token);
        // Assert
        var result = WindowsCredentialManager.IsExists(account);
        Assert.That(result, Is.True);
    }

    [Test]
    public void A07_IsExists_InvalidAccount_ReturnsFalse()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        var account = "invalid-account";
        // Act
        var result = WindowsCredentialManager.IsExists(account);
        // Assert
        Assert.That(result, Is.False);
    }     

    [Test]
    [TestCase("test-account", "test-token")]
    [TestCase("test-account2", "test-token2")]
    public void A08_Store_multi_account(string account, string token)
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        // Act
        var result = cm.TryStoreToken(account, token);
        var isExist = WindowsCredentialManager.IsExists(account);
        // Assert
        Assert.That(result, Is.True);
        Assert.That(isExist, Is.True);
    }

    [Test]
    public void A09_Try_delete_exist_account_returns_true()
    {
        // Arrange
        var cm = new WindowsCredentialManager();
        var account = "test-account3";
        var token = "test-token3";
        // Act
        cm.TryStoreToken(account, token);
        cm.TryDeleteToken(account);
        var result = WindowsCredentialManager.IsExists(account);
        // Assert
        Assert.That(result, Is.False);
    }   
}
