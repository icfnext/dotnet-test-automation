using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Configuration;

namespace OlsonDigital.TestAutomation.Xunit
{


    /// <summary>
    /// Creates a new Data Object which reads test data from a Json file
    /// </summary>
    public class JsonDataObjectFactory
    {
        private readonly string _testName;
        private readonly TestDataType _testDataType = TestDataType.Object;


        /// <summary>
        /// Creates a new Json Data Object
        /// </summary>
        /// <param name="testDataType"></param>
        /// <param name="testName"></param>
        public JsonDataObjectFactory(TestDataType testDataType, string testName)
        {
            _testDataType = testDataType;
            _testName = testName;
        }


        /// <summary>
        /// Creates the data object
        /// </summary>
        /// <returns></returns>
        public IList<object> CreateDataObject()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{_testName}.json");

            var config = builder.Build();

            return ParseConfigurationRoot(config);
        }

        internal IList<object> ParseConfigurationRoot(IConfigurationRoot configRoot)
        {
            var toReturn = new List<object>();

            var tests = configRoot.GetSection("tests");

            foreach (var test in tests.GetChildren())
            {
                var data = test.GetSection("data");
                var name = test["name"];

                var shouldPass = false;
                bool.TryParse(test["shouldPass"], out shouldPass);

                if (_testDataType == TestDataType.Array)
                {
                    int expectedResultCount = 0;
                    int.TryParse(test["expectedResultCount"], out expectedResultCount);

                    var testData = new DynamicTestArrayData(name, shouldPass, expectedResultCount);

                    var parsedData = ParseArrayData(data);
                    if (parsedData.Item1)
                    {
                        testData.Data = parsedData.Item2;
                    }

                    toReturn.Add( testData );
                }
                else
                {
                    var testData = new DynamicTestData(name, shouldPass);

                    var parsedData = ParseObjectData(data);
                    if (parsedData.Item1)
                    {
                        testData.Data = parsedData.Item2;
                    }

                    toReturn.Add(testData);
                }
            }

            return toReturn;
        }

        internal Tuple<bool, dynamic> ParseObjectData(IConfigurationSection config)
        {
            var result = new Tuple<bool, dynamic>(false, null);

            if (config.Value == null)
            {
                var childProperties = new ExpandoObject() as IDictionary<string, object>;
                foreach (var child in config.GetChildren())
                {
                    object value = null;

                    Tuple<bool, dynamic> simpleChild = ParseChildData(child);
                    if (simpleChild.Item1)
                    {
                        value = simpleChild.Item2;
                    }
                    else
                    {
                        Tuple<bool, dynamic[]> arrayChild = ParseChildArrayData(child);
                        if (arrayChild.Item1)
                        {
                            value = arrayChild.Item2;
                        }
                    }

                    childProperties[child.Key] = value;
                }

                result = new Tuple<bool, dynamic>(true, childProperties);
            }

            return result;
        }

        internal Tuple<bool, dynamic[]> ParseArrayData(IConfigurationSection config)
        {
            var result = new Tuple<bool, dynamic[]>(false, null);

            var children = new List<dynamic>();
            foreach (var child in config.GetChildren())
            {
                int intValue = 0;
                if (int.TryParse(child.Key, out intValue))
                {
                    var d = ParseObjectData(child);

                    if (d.Item1)
                    {
                        children.Add(d.Item2);
                    }
                }
            }

            result = new Tuple<bool, dynamic[]>(true, children.ToArray());

            return result;
        }

        internal Tuple<bool, dynamic> ParseChildData(IConfigurationSection config)
        {
            dynamic toReturn = new Tuple<bool, dynamic>(false, null);

            var boolData = ParseBoolData(config);
            if (boolData.Item1)
            {
                toReturn = boolData;
            }

            var intData = ParseIntData(config);
            if (!toReturn.Item1 && intData.Item1)
            {
                toReturn = intData;
            }

            var guidData = ParseGuidData(config);
            if (!toReturn.Item1 && guidData.Item1)
            {
                toReturn = guidData;
            }

            var stringData = ParseStringData(config);
            if (!toReturn.Item1 && stringData.Item1)
            {
                toReturn = stringData;
            }

            var objectData = ParseObjectData(config);
            if (objectData.Item1)
            {
                toReturn = objectData;
            }

            return toReturn;
        }

        internal Tuple<bool, dynamic[]> ParseChildArrayData(IConfigurationSection config)
        {
            dynamic toReturn = new Tuple<bool, dynamic[]>(false, null);

            var binaryData = ParseBinaryData(config);
            if (!toReturn.Item1 && binaryData.Item1)
            {
                toReturn = binaryData;
            }

            return toReturn;
        }

        internal Tuple<bool, dynamic> ParseBoolData(IConfigurationSection config)
        {
            var result = new Tuple<bool, dynamic>(false, null);

            bool boolValue = false;
            if (bool.TryParse(config.Value, out boolValue))
            {
                result = new Tuple<bool, dynamic>(true, boolValue);
            }

            return result;
        }

        internal Tuple<bool, dynamic[]> ParseBinaryData(IConfigurationSection config)
        {
            var result = new Tuple<bool, dynamic[]>(false, null);

            if (config.Value != null)
            {
                var pattern = @"data:.*;base64,([0-9A-Za-z]\w+=)";

                Regex r = new Regex(pattern);
                Match m = r.Match(config.Value);
                if (m.Success && m.Groups.Count == 2)
                {
                    var base64 = m.Groups[1].Value;
                    byte[] binaryValue = Convert.FromBase64String(base64);

                    dynamic[] d = new dynamic[binaryValue.Length];

                    for (var i = 0; i < binaryValue.Length; i++)
                    {
                        d[i] = binaryValue[i];
                    }

                    result = new Tuple<bool, dynamic[]>(true, d);
                }
            }

            return result;
        }

        internal Tuple<bool, dynamic> ParseIntData(IConfigurationSection config)
        {
            var result = new Tuple<bool, dynamic>(false, null);

            int intValue = 0;
            if (int.TryParse(config.Value, out intValue))
            {
                result = new Tuple<bool, dynamic>(true, intValue);
            }

            return result;
        }

        internal Tuple<bool, dynamic> ParseGuidData(IConfigurationSection config)
        {
            var result = new Tuple<bool, dynamic>(false, null);

            Guid guidValue = Guid.Empty;
            if (Guid.TryParse(config.Value, out guidValue))
            {
                result = new Tuple<bool, dynamic>(true, guidValue);
            }

            return result;
        }

        internal Tuple<bool, dynamic> ParseStringData(IConfigurationSection config)
        {
            var result = new Tuple<bool, dynamic>(false, null);

            dynamic stringValue = config.Value?.ToString();

            if (!string.IsNullOrEmpty(stringValue))
            {
                //Skip anything that is a binary string
                var pattern = @"data:.*;base64,([0-9A-Za-z]\w+=)";

                Regex r = new Regex(pattern);
                Match m = r.Match(config.Value);
                if (!m.Success)
                {
                    result = new Tuple<bool, dynamic>(true, stringValue);
                }
            }

            return result;
        }

    }
}
