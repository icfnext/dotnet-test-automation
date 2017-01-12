using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit.Sdk;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// Generates new Xunit test data based on the provided type
    /// </summary>
    public class ReflectionDataAttribute : DataAttribute
    {
        private readonly ReflectionDataObjectFactory _dataObjectFactory;


        /// <summary>
        /// Generates new Xunit test data based on the provided type
        /// </summary>
        /// <param name="testDataType"></param>
        /// <param name="name"></param>
        /// <param name="shouldPass"></param>
        /// <param name="type"></param>
        /// <param name="numberOfRows"></param>
        public ReflectionDataAttribute(TestDataType testDataType, string name, bool shouldPass, Type type, int numberOfRows = 1)
        {
            _dataObjectFactory = new ReflectionDataObjectFactory(testDataType, name, shouldPass, type, numberOfRows);
        }

        /// <summary>
        /// Gets the data for the test case
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var toReturn = new List<object[]>();

            var testData = _dataObjectFactory.GetDataObject();

            toReturn.Add(new object[] { testData });

            return toReturn;
        }
    }
}