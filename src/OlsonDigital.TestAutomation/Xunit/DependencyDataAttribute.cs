using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit.Sdk;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// A Data Object that is used to provide access to a type and mock it's depedencies
    /// </summary>
    public class DependencyDataAttribute : DataAttribute
    {
        private readonly DependencyDataObjectFactory _dataObjectFactory;


        /// <summary>
        /// Creates the attribute
        /// </summary>
        /// <param name="type"></param>
        public DependencyDataAttribute(Type type)
        {
            _dataObjectFactory = new DependencyDataObjectFactory(type);
        }


        /// <summary>
        /// Gets the data object for a test
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var toReturn = new List<object[]>();

            var dataObjectFactory = _dataObjectFactory.CreateDataObject();

            toReturn.Add(new object[] { dataObjectFactory });

            return toReturn;
        }
    }
}