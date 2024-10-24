
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
            var testObject = new TestClass { Name = "Test", Age = 30 };

            Saver.Serialize(testObject, TestFilePath);

            Assert.That(File.Exists(TestFilePath).Equals(true));
        }

        [Test]
        public void Serialize_ShouldWriteIndentedJson()
        {
            var testObject = new TestClass { Name = "Test", Age = 30 };

            Saver.Serialize(testObject, TestFilePath);
            string json = File.ReadAllText(TestFilePath);

            Assert.That(json.Contains(Environment.NewLine).Equals(true)); // Checks if it's indented
        }

        [Test]
        public void Deserialize_ShouldReturnCorrectObject()
        {
            var testObject = new TestClass { Name = "Test", Age = 30 };
            Saver.Serialize(testObject, TestFilePath);

            var result = Saver.Deserialize<TestClass>(TestFilePath);

            Assert.That(result != null);
            Assert.That(testObject.Name.Equals(result.Name));
            Assert.That(testObject.Age.Equals(result.Age));
        }

        [Test]
        public void Serialize_Enum_ShouldHandleEnumSerialization()
        {
            var enumObject = TestEnum.ValueOne;

            Saver.Serialize(enumObject, TestFilePath);

            string json = File.ReadAllText(TestFilePath);
            Assert.That(json.Contains("ValueOne").Equals(true));
        }

        [Test]
        public void Deserialize_Enum_ShouldHandleEnumDeserialization()
        {
            var enumObject = TestEnum.ValueOne;
            Saver.Serialize(enumObject, TestFilePath);

            var result = Saver.Deserialize<TestEnum>(TestFilePath);

            Assert.That(TestEnum.ValueOne.Equals(result));
        }

        [Test]
        public void Deserialize_FileNotFound_ShouldThrowFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => Saver.Deserialize<TestClass>("nonexistentfile.json"));
        }

        [Test]
        public void Deserialize_InvalidJson_ShouldThrowInvalidOperationException()
        {
            File.WriteAllText(TestFilePath, "invalid json");
            
            Assert.Throws<JsonException>(() => Saver.Deserialize<TestClass>(TestFilePath));
        }
    }
}
