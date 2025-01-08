// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppVeyor.RestApi.Extensions;

[TestFixture]
internal class StringExtTest
{
    [Test]
    public void AsString_WithValidInput_ReturnsJoinedString()
    {
        var input = new List<string> { "one", "two", "three" };
        var result = input.AsString(",");
        Assert.That(result, Is.EqualTo("one,two,three"));
    }

    [Test]
    public void AsString_WithEmptyInput_ReturnsEmptyString()
    {
        var input = new List<string>();
        var result = input.AsString(",");
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void SplitString_WithSeparator_ReturnsKeyValuePair()
    {
        var input = "key:value";
        var result = input.SplitString(':');
        Assert.That(result, Is.EqualTo(("key", "value")));
    }

    [Test]
    public void SplitString_WithoutSeparator_ReturnsOriginalString()
    {
        var input = "keyvalue";
        var result = input.SplitString(':');
        Assert.That(result, Is.EqualTo((input, string.Empty)));
    }

    [Test]
    public void FullException_WithInnerException_ReturnsFullMessage()
    {
        var innerException = new Exception("Inner exception");
        var exception = new Exception("Outer exception", innerException);
        var result = exception.FullException();
        Assert.That(result, Is.EqualTo("Outer exception\nInnerException: Inner exception"));
    }

    [Test]
    public void Q_WithValidString_ReturnsQuotedString()
    {
        var input = "test";
        var result = input.Q();
        Assert.That(result, Is.EqualTo("\"test\""));
    }

    [Test]
    public void AddQueryString_WithValidParameters_ReturnsUriWithQueryString()
    {
        var uri = "http://example.com";
        var result = uri.AddQueryString("key", "value");
        Assert.That(result, Is.EqualTo("http://example.com?key=value"));
    }

    [Test]
    public void AddQueryString_WithDictionary_ReturnsUriWithQueryString()
    {
        var uri = "http://example.com";
        var values = new Dictionary<string, object> { { "key1", "value1" }, { "key2", "value2" } };
        var result = uri.AddQueryString(values);
        Assert.That(result, Is.EqualTo("http://example.com?key1=value1&key2=value2"));
    }

    [Test]
    public void Enum2String_WithValidEnum_ReturnsEnumName()
    {
        var result = TestEnum.Value1.Enum2String();
        Assert.That(result, Is.EqualTo("Value1"));
    }

    private enum TestEnum
    {
        Value1,
        Value2
    }
}
