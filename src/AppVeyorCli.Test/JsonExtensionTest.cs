// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyorCli.Test;

using AppVeyor.RestApi.Extensions;
using AppVeyor.Test.Extensions;

internal class JsonExtensionTest
{
    [Test]
    public void AsStringTest()
    {
        string[] args = ["one", "two", "three"];
        var result = args.AsString();
        Assert.That(result, Is.EqualTo("one,two,three"));
    }

    [Test]
    public void ToObject_NullJson_ReturnsDefault()
    {
        // Arrange
        string? json = null;

        // Act
        var result = json?.ToObject<object>();

        // Assert
        Assert.That(result, Is.EqualTo(default));
    }

    [Test]
    public void ToObject_EmptyJson_ReturnsDefault()
    {
        // Arrange
        string json = "";

        // Act
        var result = json.ToObject<object>();

        // Assert
        Assert.That(result, Is.EqualTo(default(object)));
    }

    [Test]
    public void ToObject_ValidJson_ReturnsDeserializedObject()
    {
        // Arrange
        var json = "{\"name\":\"John\",\"age\":30}";

        // Act
        var result = json.ToObject<Person>();

        // Assert
        Assert.That(result?.Name, Is.EqualTo("John"));
        Assert.That(result?.Age, Is.EqualTo(30));
    }

    [Test]
    public void ToJson_NullObject_ReturnsEmptyJson()
    {
        // Arrange
        object? obj = null;

        // Act
        var result = obj?.ToJson();

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }

    [Test]
    public void ToJson_ValidObject_ReturnsSerializedJson()
    {
        // Arrange
        var person = new Person { Name = "John", Age = 30 };

        // Act
        var result = person.ToJson();

        // Assert
        Assert.That(result, Is.EqualTo("""{"Name":"John","Age":30}"""));
    }

    [Test]
    public void JsonFormat_InvalidJson_ReturnsOriginalJson()
    {
        // Arrange
        string json = "invalid json";

        // Act
        var result = json.JsonFormat();

        // Assert
        Assert.That(result, Is.EqualTo(json));
    }

    [Test]
    public void JsonFormat_ValidJson_ReturnsFormattedJson()
    {
        // Arrange
        string json = """{"name":"John","age":30}""";

        // Act
        var result = json.JsonFormat();

        // Assert
        Assert.That(result.RemoveSpaces(), Is.EqualTo(
            """
              {
                "name": "John",
                "age": 30
              }
          """.RemoveSpaces()));
    }

#nullable disable
    private class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
#nullable restore
}


