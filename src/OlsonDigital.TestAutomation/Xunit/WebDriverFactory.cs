
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace OlsonDigital.TestAutomation.Xunit
{
    /// <summary>
    /// A Factory Method for creating Web Drivers
    /// </summary>
    public class WebDriverFactory
    { 

        /// <summary>
        /// Creates a new Web Driver for the targeted browser
        /// </summary>
        /// <param name="testConfig">The current test config</param>
        /// <param name="targetBrowser">The target browser</param>
        /// <param name="testName">The name of the test.  This is used for Remote Web Drivers</param>
        /// <returns>The newly created WebDriver</returns>
        public static IWebDriver CreateWebDriver(TestConfig testConfig, TargetBrowser targetBrowser, string testName)
        {
            IWebDriver toReturn = null;

            if ((targetBrowser & TargetBrowser.Chrome) == TargetBrowser.Chrome)
            {
                toReturn = new ChromeDriver();
            }
            if ((targetBrowser & TargetBrowser.InternetExplorer) == TargetBrowser.InternetExplorer)
            {
                toReturn = new InternetExplorerDriver();
            }
            else if ((targetBrowser & TargetBrowser.RemoteInternetExplorer) == TargetBrowser.RemoteInternetExplorer)
            {
                toReturn = CreateRemoteWebDriver(
                    TargetBrowser.RemoteInternetExplorer, 
                    testConfig, 
                    DesiredCapabilities.InternetExplorer(), 
                    testName);
            }
            else if ((targetBrowser & TargetBrowser.RemoteChrome) == TargetBrowser.RemoteChrome)
            {
                toReturn = CreateRemoteWebDriver(
                    TargetBrowser.RemoteChrome, 
                    testConfig, 
                    DesiredCapabilities.Chrome(), 
                    testName);
            }

            return toReturn;
        }

        internal static IWebDriver CreateRemoteWebDriver(
            TargetBrowser targetBrowser, 
            TestConfig testConfig, 
            DesiredCapabilities capability, 
            string testName)
        {
            IWebDriver toReturn = null;
            var remoteDriverConfig = testConfig.RemoteWebDriverConfig;

            if (remoteDriverConfig?.Capabiliities.ContainsKey(targetBrowser) == true)
            {
                var capabilities = remoteDriverConfig.Capabiliities[targetBrowser];
                foreach (var key in capabilities.Keys)
                {
                    capability.SetCapability(key, capabilities[key]);
                }

                if (!string.IsNullOrEmpty(testConfig.BuildNumber))
                {
                    capability.SetCapability("build", testConfig.BuildNumber);
                }

                if (!string.IsNullOrEmpty(testName))
                {
                    capability.SetCapability("name", testName);
                }

                capability.SetCapability("browserstack.user", remoteDriverConfig.User);
                capability.SetCapability("browserstack.key", remoteDriverConfig.Password);

                toReturn = new RemoteWebDriver(remoteDriverConfig.Url, capability);
            }

            return toReturn;
        }
    }
}