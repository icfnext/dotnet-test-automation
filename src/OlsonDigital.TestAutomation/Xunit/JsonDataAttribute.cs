using System.Collections.Generic;
using System.Reflection;

using Xunit.Sdk;

namespace OlsonDigital.TestAutomation.Xunit
{
    /// <summary>
    /// An Xunit Data Attribute to read from Json files
    /// </summary>
    public class JsonDataAttribute : DataAttribute
    {
        private readonly JsonDataObjectFactory _dataObjectFactory;

        /// <summary>
        /// Creates a new Data Attribute
        /// </summary>
        /// <param name="testDataType"></param>
        /// <param name="testName"></param>
        public JsonDataAttribute(TestDataType testDataType, string testName)
        {
            _dataObjectFactory = new JsonDataObjectFactory(testDataType, testName);
        }


        /// <summary>
        /// Gets the data for the test case
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var toReturn = new List<object[]>();

            var dataObject = _dataObjectFactory.CreateDataObject();

            // Ok we will get back one row per test
            foreach (var jsonTestData in dataObject)
            {
                toReturn.Add(new object[] { jsonTestData });
            }

            return toReturn;
        }
    }
}