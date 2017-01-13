using System;

using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace OlsonDigital.TestAutomation.Xunit
{
    /// <summary>
    /// The current confgiuration for Tests
    /// </summary>
    public class TestConfig
    {
        /// <summary>
        /// Creates a new Test Config Object
        /// </summary>
        /// <param name="configRoot">A Configuration Root to read from.</param>
        public TestConfig(IConfigurationRoot configRoot)
        {
            _browser = (TargetBrowser)Enum.Parse(typeof(TargetBrowser), configRoot["TargetBrowsers"]);
            _buildNumber = configRoot["BuildNumber"];

            _remoteWebDriverConfig = RemoteWebDriverConfig.Hydrate(configRoot.GetSection("RemoteWebDriver"));

            var assemblies = configRoot.GetSection("AssembliesToLoad");
            foreach(var a in assemblies.GetChildren())
            {
                _targetAssemblies.Add(a.Value);
            }
        }


        private TargetBrowser _browser;
        private RemoteWebDriverConfig _remoteWebDriverConfig;
        private string _buildNumber;
        private IList<string> _targetAssemblies = new List<string>();


        /// <summary>
        /// The Current Build Number
        /// </summary>
        public string BuildNumber => _buildNumber;

        /// <summary>
        /// The Browsers that we are Targeting
        /// </summary>
        public TargetBrowser Browser => _browser;

        /// <summary>
        /// Configuration for the remote webdriver(s)
        /// </summary>
        public RemoteWebDriverConfig RemoteWebDriverConfig => _remoteWebDriverConfig;

        /// <summary>
        /// A list of Assemblies to Load Types from
        /// </summary>
        public IList<string> TargetAssemblies => _targetAssemblies;
    }
}