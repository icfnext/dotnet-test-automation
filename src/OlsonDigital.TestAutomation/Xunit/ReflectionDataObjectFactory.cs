using System;
using System.Collections.Generic;
using System.Dynamic;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// Creates new Data Objects using Reflection
    /// </summary>
    public class ReflectionDataObjectFactory
    {

        private readonly string _name;
        private readonly bool _shouldPass;
        private readonly Type _type;
        private readonly int _numberOfRows;
        private readonly TestDataType _testDataType = TestDataType.Object;


        /// <summary>
        /// Generates new Xunit test data based on the provided type
        /// </summary>
        /// <param name="testDataType"></param>
        /// <param name="name"></param>
        /// <param name="shouldPass"></param>
        /// <param name="type"></param>
        /// <param name="numberOfRows"></param>
        public ReflectionDataObjectFactory(TestDataType testDataType, string name, bool shouldPass, Type type, int numberOfRows = 1)
        {
            _testDataType = testDataType;
            _name = name;
            _shouldPass = shouldPass;
            _type = type;
            _numberOfRows = numberOfRows;
        }


        /// <summary>
        /// Gets the data for the test case
        /// </summary>
        /// <returns></returns>
        public object GetDataObject()
        {
            object toReturn = null;

            var testData = GenerateTestData();

            if (_testDataType == TestDataType.Array)
            {
                var dynamicData = new DynamicTestArrayData(_name, _shouldPass, _numberOfRows);

                dynamicData.Data = testData;

                toReturn = dynamicData;
            }
            else if (_testDataType == TestDataType.Object)
            {
                var dynamicData = new DynamicTestData(_name, _shouldPass);

                //assume the first result
                dynamicData.Data = testData[0];

                toReturn = dynamicData;
            }

            return toReturn;
        }


        internal dynamic[] GenerateTestData()
        {
            dynamic[] toReturn = new dynamic[_numberOfRows];

            var properties = _type.GetProperties();
            for (int row = 0; row < _numberOfRows; row++)
            {
                var dynamicData = new ExpandoObject() as IDictionary<string, object>;

                for (int i = 0; i < properties.Length; i++)
                {
                    var prop = properties[i];

                    if (prop.PropertyType == typeof(string))
                    {
                        dynamicData[prop.Name] = $"{row}_{prop.Name}";
                    }
                    else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                    {
                        dynamicData[prop.Name] = i % 2 == 0;
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                    {
                        dynamicData[prop.Name] = row + i;
                    }
                    else if (prop.PropertyType == typeof(Guid) || prop.PropertyType == typeof(Guid?))
                    {
                        dynamicData[prop.Name] = Guid.NewGuid();
                    }
                    else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        dynamicData[prop.Name] = DateTime.Now;
                    }
                }

                toReturn[row] = dynamicData;
            }

            return toReturn;
        }

    }
}
