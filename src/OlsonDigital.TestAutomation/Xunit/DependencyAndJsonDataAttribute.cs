using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit.Sdk;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// Combines the Mock Depdendcy Data Object with a Json Data Object
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DependencyAndJsonDataAttribute : DataAttribute
    {
        private readonly Type _type;
        private readonly TestDataType _testDataType;
        private readonly string _testName;

        /// <summary>
        /// Creates the Attribute
        /// </summary>
        /// <param name="type">The type to create and Mock it's depedencies</param>
        /// <param name="testDataType">The type of dynamic object required</param>
        /// <param name="testName">The name/path to the json file with the test data</param>
        public DependencyAndJsonDataAttribute(Type type, TestDataType testDataType, string testName)
        {
            _type = type;
            _testDataType = testDataType;
            _testName = testName;
        }


        /// <summary>
        /// Gets the data for a test.  The first param will be the depedency data object, the second param will be the json data object.
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var toReturn = new List<object[]>();

            var jsonDataObjectFactory = new JsonDataObjectFactory(_testDataType, _testName);

            var jsonDataObject = jsonDataObjectFactory.CreateDataObject();

            // Ok we will get back one row per test
            foreach(var jsonTestData in jsonDataObject )
            {
                //Need to create the factory in the loop so it exists once per json test data set
                var dependencyDataObjectFactory = new DependencyDataObjectFactory(_type);
                var depedencyDataObject = dependencyDataObjectFactory.CreateDataObject();

                toReturn.Add(new object[] { depedencyDataObject, jsonTestData });
            }

            return toReturn;
        }
    }
}