using System;

using Microsoft.Extensions.Configuration;

using Xunit;

using OlsonDigital.TestAutomation.Xunit;

namespace OlsonDigital.TestAutomation.Tests.Xunit
{
    public class JsonDataObjectFactoryTests
    {
        [Category("Test Extensions")]
        [Theory]
        [InlineData("ParseConfigurationRoot-1", true, 2)]
        public void ParseConfigurationRoot(string jsonFile, bool expectSuccess, int expectedCount)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"TestAssets/Xunit/JsonDataObjectFactory/ParseConfigurationRoot/{jsonFile}.json");

            var config = builder.Build();

            var attr = new JsonDataObjectFactory(TestDataType.Array, "");

            var result = attr.ParseConfigurationRoot(config);

            if (expectSuccess)
            {
                Assert.NotNull(result);
                Assert.Equal(1, result.Count);

                var object1 = result[0] as DynamicTestArrayData;
                Assert.NotNull(object1);
                Assert.Equal(2, object1.ExpectedResultCount);
                Assert.True(object1.ShouldPass);
                Assert.Equal(new Guid("e26528c7-daa3-4bf5-83b8-ce9ae9098b62"), object1.Data[0].id);
                Assert.Equal(5, object1.Data[0].count);
                Assert.Equal("Hello World", object1.Data[0].name);
                Assert.Equal(true, object1.Data[0].shouldPass);

                Assert.Equal(new Guid("9f1a52c4-0b49-4075-a3fd-864fc037a7e6"), object1.Data[1].id);
                Assert.Equal(10, object1.Data[1].count);
                Assert.Equal("Hello World 2", object1.Data[1].name);
                Assert.Equal(false, object1.Data[1].shouldPass);
            }
        }


        [Category("Test Extensions")]
        [Theory]
        [InlineData("ParseArrayData-1", true, 2)]
        public void ParseArrayData(string jsonFile, bool expectSuccess, int expectedCount)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"TestAssets/Xunit/JsonDataObjectFactory/ParseArrayData/{jsonFile}.json");

            var config = builder.Build();

            var attr = new JsonDataObjectFactory(TestDataType.Array, "");

            var data = config.GetSection("data");
            var result = attr.ParseArrayData(data);

            Assert.Equal(expectSuccess, result.Item1);

            if (expectSuccess)
            {
                Assert.NotNull(result.Item2);
                Assert.Equal(2, result.Item2.Length);


                Assert.Equal(new Guid("e26528c7-daa3-4bf5-83b8-ce9ae9098b62"), result.Item2[0].id);
                Assert.Equal(5, result.Item2[0].count);
                Assert.Equal("Hello World", result.Item2[0].name);
                Assert.Equal(true, result.Item2[0].shouldPass);

                Assert.Equal(new Guid("9f1a52c4-0b49-4075-a3fd-864fc037a7e6"), result.Item2[1].id);
                Assert.Equal(10, result.Item2[1].count);
                Assert.Equal("Hello World 2", result.Item2[1].name);
                Assert.Equal(false, result.Item2[1].shouldPass);
            }
        }


        [Category("Test Extensions")]
        [Theory]
        [InlineData("ParseObjectData-1", true)]
        public void ParseObjectData(string jsonFile, bool expectSuccess)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"TestAssets/Xunit/JsonDataObjectFactory/ParseObjectData/{jsonFile}.json");

            var config = builder.Build();

            var attr = new JsonDataObjectFactory(TestDataType.Object, "");

            var data = config.GetSection("data");
            var binaryValue = data.GetSection("binaryValue");
            var result = attr.ParseObjectData(data);

            Assert.Equal(expectSuccess, result.Item1);

            if (expectSuccess)
            {
                Assert.NotNull(result.Item2);
                Assert.Equal(new Guid("e26528c7-daa3-4bf5-83b8-ce9ae9098b62"), result.Item2.id);
                Assert.Equal(5, result.Item2.count);
                Assert.Equal("Hello World", result.Item2.name);
                Assert.Equal(true, result.Item2.shouldPass);

                Assert.NotNull(result.Item2.child);
                Assert.Equal(10, result.Item2.child.count);
                Assert.Equal("Hello World 2", result.Item2.child.name);

                string base64 = binaryValue.Value.Substring("data:application/octet-stream;base64,".Length);
                byte[] expectedBytes = Convert.FromBase64String(base64);

                Assert.Equal(expectedBytes.Length, result.Item2.binaryValue.Length);
                for (int i = 0; i < expectedBytes.Length; i++)
                {
                    Assert.Equal(expectedBytes[i], result.Item2.binaryValue[i]);
                }
            }
        }


        [Category("Test Extensions")]
        [Theory]
        [InlineData("ParseChildData-1", true)]
        public void ParseChildData(string jsonFile, bool expectSuccess)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"TestAssets/Xunit/JsonDataObjectFactory/ParseChildData/{jsonFile}.json");

            var config = builder.Build();

            var attr = new JsonDataObjectFactory(TestDataType.Object, "");

            var data = config.GetSection("data");
            var result = attr.ParseChildData(data);

            Assert.Equal(expectSuccess, result.Item1);

            if (expectSuccess)
            {
                Assert.NotNull(result.Item2);
                Assert.Equal(new Guid("e26528c7-daa3-4bf5-83b8-ce9ae9098b62"), result.Item2.id);
                Assert.Equal(5, result.Item2.count);
                Assert.Equal("Hello World", result.Item2.name);
                Assert.Equal(true, result.Item2.shouldPass);

                Assert.NotNull(result.Item2.child);
                Assert.Equal(10, result.Item2.child.count);
                Assert.Equal("Hello World 2", result.Item2.child.name);
            }
        }


        [Category("Test Extensions")]
        [Theory]
        [InlineData("ParseBoolData-1", true, true)]
        [InlineData("ParseBoolData-2", true, false)]
        [InlineData("ParseBoolData-3", false, false)]
        public void ParseBoolData(string jsonFile, bool expectSuccess, bool expected)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"TestAssets/Xunit/JsonDataObjectFactory/ParseBoolData/{jsonFile}.json");

            var config = builder.Build();

            var attr = new JsonDataObjectFactory(TestDataType.Object, "");

            var data = config.GetSection("data");
            var result = attr.ParseBoolData(data.GetSection("shouldPass"));

            Assert.Equal(expectSuccess, result.Item1);

            if(expectSuccess)
            {
                Assert.Equal(expected, result.Item2);
            }
        }


        [Category("Test Extensions")]
        [Theory]
        [InlineData("ParseIntData-1", true, 5)]
        [InlineData("ParseIntData-2", false, 0)]
        public void ParseIntData(string jsonFile, bool expectSuccess, int expected)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"TestAssets/Xunit/JsonDataObjectFactory/ParseIntData/{jsonFile}.json");

            var config = builder.Build();

            var attr = new JsonDataObjectFactory(TestDataType.Object, "");

            var data = config.GetSection("data");
            var result = attr.ParseIntData(data.GetSection("count"));

            Assert.Equal(expectSuccess, result.Item1);

            if (expectSuccess)
            {
                Assert.Equal(expected, result.Item2);
            }
        }


        [Category("Test Extensions")]
        [Theory]
        [InlineData("ParseGuidData-1", true, "e26528c7-daa3-4bf5-83b8-ce9ae9098b62")]
        [InlineData("ParseGuidData-2", false, "")]
        public void ParseGuidData(string jsonFile, bool expectSuccess, string expected)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"TestAssets/Xunit/JsonDataObjectFactory/ParseGuidData/{jsonFile}.json");

            var config = builder.Build();

            var attr = new JsonDataObjectFactory(TestDataType.Object, "");

            var data = config.GetSection("data");
            var result = attr.ParseGuidData(data.GetSection("id"));

            Assert.Equal(expectSuccess, result.Item1);

            if (expectSuccess)
            {
                Assert.Equal(new Guid(expected), result.Item2);
            }
        }


        [Category("Test Extensions")]
        [Theory]
        [InlineData("ParseStringData-1", true, "Hello World")]
        public void ParseStringData(string jsonFile, bool expectSuccess, string expected)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"TestAssets/Xunit/JsonDataObjectFactory/ParseStringData/{jsonFile}.json");

            var config = builder.Build();

            var attr = new JsonDataObjectFactory(TestDataType.Object, "");

            var data = config.GetSection("data");
            var result = attr.ParseStringData(data.GetSection("name"));

            Assert.Equal(expectSuccess, result.Item1);

            if (expectSuccess)
            {
                Assert.Equal(expected, result.Item2);
            }
        }


        [Category("Test Extensions")]
        [Theory]
        [InlineData("ParseBinaryData-1", true)]
        public void ParseBinaryData(string jsonFile, bool expectSuccess)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"TestAssets/Xunit/JsonDataObjectFactory/ParseBinaryData/{jsonFile}.json");

            var config = builder.Build();

            var attr = new JsonDataObjectFactory(TestDataType.Object, "");

            var data = config.GetSection("data");
            var name = data.GetSection("name");
            var result = attr.ParseBinaryData(name);

            string base64 = name.Value.Substring("data:application/octet-stream;base64,".Length);
            byte[] expectedBytes = Convert.FromBase64String(base64);

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);

            Assert.Equal(expectedBytes.Length, result.Item2.Length);

            for(int i=0; i < expectedBytes.Length; i++)
            {
                Assert.Equal(expectedBytes[i], result.Item2[i]);
            }
        }
    }
}