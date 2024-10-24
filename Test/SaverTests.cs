
using NUnit.Framework;
using System.IO;
using System.Text.Json;
using CashInn.Helper;
using System;

namespace CashInn.Test

{
    public class SaverTests
    {
        private class TestClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private enum TestEnum
        {
            ValueOne,
            ValueTwo
        }

        private const string TestFilePath = "testfile.json";

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [Test]
        public void Serialize_ShouldCreateJsonFile()
        {
            // Arrange
            var testObject = new TestClass { Name = "Test", Age = 30 };

            // Act
            Saver.Serialize(testObject, TestFilePath);

            // Assert
            Assert.That(File.Exists(TestFilePath).Equals(true));
        }

        [Test]
        public void Serialize_ShouldWriteIndentedJson()
        {
            // Arrange
            var testObject = new TestClass { Name = "Test", Age = 30 };

            // Act
            Saver.Serialize(testObject, TestFilePath);
            string json = File.ReadAllText(TestFilePath);

            // Assert
            Assert.That(json.Contains(Environment.NewLine).Equals(true)); // Checks if it's indented
        }

        [Test]
        public void Deserialize_ShouldReturnCorrectObject()
        {
            // Arrange
            var testObject = new TestClass { Name = "Test", Age = 30 };
            Saver.Serialize(testObject, TestFilePath);

            // Act
            var result = Saver.Deserialize<TestClass>(TestFilePath);

            // Assert
            Assert.That(result != null);
            Assert.That(testObject.Name.Equals(result.Name));
            Assert.That(testObject.Age.Equals(result.Age));
        }

        [Test]
        public void Serialize_Enum_ShouldHandleEnumSerialization()
        {
            // Arrange
            var enumObject = TestEnum.ValueOne;

            // Act
            Saver.Serialize(enumObject, TestFilePath);

            // Assert
            string json = File.ReadAllText(TestFilePath);
            Assert.That(json.Contains("ValueOne").Equals(true));
        }

        [Test]
        public void Deserialize_Enum_ShouldHandleEnumDeserialization()
        {
            // Arrange
            var enumObject = TestEnum.ValueOne;
            Saver.Serialize(enumObject, TestFilePath);

            // Act
            var result = Saver.Deserialize<TestEnum>(TestFilePath);

            // Assert
            Assert.That(TestEnum.ValueOne.Equals(result));
        }

        [Test]
        public void Deserialize_FileNotFound_ShouldThrowFileNotFoundException()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => Saver.Deserialize<TestClass>("nonexistentfile.json"));
        }

        [Test]
        public void Deserialize_InvalidJson_ShouldThrowInvalidOperationException()
        {
            // Arrange
            File.WriteAllText(TestFilePath, "invalid json");

            // Act & Assert
            Assert.Throws<JsonException>(() => Saver.Deserialize<TestClass>(TestFilePath));
        }
    }
}
