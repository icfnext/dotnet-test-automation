using Autofac;
using OlsonDigital.TestAutomation.IoC;
using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit.Sdk;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// Adds Selenium Configuration with a Json Data Object
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SeleniumAndJsonDataAttribute : DataAttribute
    {
        private IContainer _container = ContainerFactory.Container;
        private readonly TargetBrowser _targetBrowser;
        private readonly TestDataType _testDataType;
        private readonly string _jsonPath;

        /// <summary>
        /// Creates the Attribute
        /// </summary>
        /// <param name="targetBrowser">The names of the selenium configs to use</param>
        /// <param name="testDataType">The type of dynamic object required</param>
        /// <param name="jsonPath">The name/path to the json file with the test data</param>
        public SeleniumAndJsonDataAttribute(TargetBrowser targetBrowser, TestDataType testDataType, string jsonPath)
        {
            _targetBrowser = targetBrowser;
            _testDataType = testDataType;
            _jsonPath = jsonPath;
        }


        /// <summary>
        /// Gets the data for a test.  The first param will be the depedency data object, the second param will be the json data object.
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var toReturn = new List<object[]>();

            var jsonDataObjectFactory = new JsonDataObjectFactory(_testDataType, _jsonPath);

            var jsonDataObject = jsonDataObjectFactory.CreateDataObject();

            // Ok we will get back one row per test
            foreach(var jsonTestData in jsonDataObject )
            {
                var targetBrowsers = SplitValue(_targetBrowser);
                foreach (var targetBrowser in targetBrowsers)
                {
                    toReturn.Add(new object[] { targetBrowser, jsonTestData });
                }
            }

            return toReturn;
        }


        internal IEnumerable<TargetBrowser> SplitValue(TargetBrowser value)
        {
            TestConfig testConfig = _container.Resolve<TestConfig>();

            if (IsFlagOn<TargetBrowser>(testConfig.Browser, value, TargetBrowser.Chrome))
            {
                yield return TargetBrowser.Chrome;
            }

            if (IsFlagOn<TargetBrowser>(testConfig.Browser, value, TargetBrowser.InternetExplorer))
            {
                yield return TargetBrowser.InternetExplorer;
            }

            if (IsFlagOn<TargetBrowser>(testConfig.Browser, value, TargetBrowser.RemoteInternetExplorer))
            {
                yield return TargetBrowser.RemoteInternetExplorer;
            }

            if (IsFlagOn<TargetBrowser>(testConfig.Browser, value, TargetBrowser.RemoteChrome))
            {
                yield return TargetBrowser.RemoteChrome;
            }
        }


        internal static bool IsFlagOn<T>(T configValue, T dataValue, T desiredFlag) where T : struct
            => IsFlagOn(configValue, desiredFlag) && IsFlagOn(dataValue, desiredFlag);

        internal static bool IsFlagOn<T>(T value, T desiredFlag) where T : struct
            => (((int)(object)value & (int)(object)desiredFlag) == (int)(object)desiredFlag);
    }
}