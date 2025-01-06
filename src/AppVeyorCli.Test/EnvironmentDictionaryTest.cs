using NUnit.Framework;
using AppVeyor.Api.Collection;
using System.Collections.Generic;
using AppVeyor.Test;
using AppVeyor.Test.Extensions;

namespace AppVeyor.Test
{
    [TestFixture]
    public class EnvironmentDictionaryTest
    {
        [Test]
        public void Constructor_with_no_parameters_should_initialize_emptydictionary()
        {
            // Arrange & Act
            var sut = new EnvironmentDictionary();

            // Assert
            Assert.That(sut, Is.Empty);
        }

        [Test]
        public void Constructor_with_stringarray_should_parse_and_add_items()
        {
            // Arrange
            var items = new[] { "key1:value1", "key2:value2" };

            // Act
            var sut = new EnvironmentDictionary(items);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(sut.Count, Is.EqualTo(2));
                Assert.That(sut["key1"], Is.EqualTo("value1"));
                Assert.That(sut["key2"], Is.EqualTo("value2"));
            });
        }

        [Test]
        public void Constructor_with_environment_varenumerable_should_add_items()
        {
            // Arrange
            var items = new List<EnvironmentVar>
            {
                new EnvironmentVar("key1", "value1"),
                new EnvironmentVar("key2", "value2")
            };

            // Act
            var sut = new EnvironmentDictionary(items);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(sut.Count, Is.EqualTo(2));
                Assert.That(sut["key1"], Is.EqualTo("value1"));
                Assert.That(sut["key2"], Is.EqualTo("value2"));
            });
        }

        [Test]
        public void Parsestring_validstring_should_return_environmentvar()
        {
            // Arrange
            var input = "key1:value1";

            // Act
            var sut = new EnvironmentDictionary(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(sut, Is.Not.Null);
                Assert.That(sut["key1"], Is.EqualTo("value1"));
            });
        }

        [Test]
        [TestCase(":")]
        [TestCase("=")]
        public void ParseString_with_dot_dashString_ShouldReturnEnvironmentVar(string s)
        {
            // Arrange
            string[] input = [
                $"key1{s}value1",
                $"key2{s}a-b-c",
                $"key3{s}a.b.c",
                $@"key4{s}c:\build\a.b.c",
                $"key5{s}./build/pkg.1.2.3-dev.123.nupkg",
                $@"key6{s}.\build\pkg.1.2.3-dev.123.nupkg",
                $"key7{s}\"a b c\""];

            // Act
            var sut = new EnvironmentDictionary(input);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(sut, Is.Not.Null);
                Assert.That(sut["key1"], Is.EqualTo("value1"));
                Assert.That(sut["key2"], Is.EqualTo("a-b-c"));
                Assert.That(sut["key3"], Is.EqualTo("a.b.c"));
                Assert.That(sut["key4"], Is.EqualTo("c:\\build\\a.b.c"));
                Assert.That(sut["key5"], Is.EqualTo("./build/pkg.1.2.3-dev.123.nupkg"));
                Assert.That(sut["key6"], Is.EqualTo(".\\build\\pkg.1.2.3-dev.123.nupkg"));
                Assert.That(sut["key7"], Is.EqualTo("a b c"));
            });
        }

        [Test]
        public void Parsestring_invalid_string_should_return_empty()
        {
            // Arrange
            var input = "invalid_string";

            // Act
            var sut = new EnvironmentDictionary(input);

            // Assert
            Assert.That(sut["invalid_string"], Is.Empty);
        }
    }
}

