using System;

using Microsoft.Extensions.Configuration;

namespace OlsonDigital.TestAutomation.Xunit
{
    public class TestConfig
    {
        private static Lazy<TestConfig> _singleton = new Lazy<TestConfig>(() =>
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile("test-config.json", false, true);
            configBuilder.AddEnvironmentVariables();

            var configRoot = configBuilder.Build();

            var config = new TestConfig
            {
                _browser = (TargetBrowser)Enum.Parse(typeof(TargetBrowser), configRoot["TargetBrowsers"]),
                _buildNumber = configRoot["BuildNumber"]
            };

            config._remoteWebDriverConfig = RemoteWebDriverConfig.Hydrate(configRoot.GetSection("RemoteWebDriver"));

            return config;
        });


        private TargetBrowser _browser;
        private RemoteWebDriverConfig _remoteWebDriverConfig;
        private string _buildNumber;


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
        /// Gets the current configuration
        /// </summary>
        public static TestConfig Instance => _singleton.Value;
    }
}