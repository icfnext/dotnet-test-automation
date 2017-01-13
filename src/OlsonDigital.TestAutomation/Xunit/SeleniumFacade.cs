using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Autofac;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace OlsonDigital.TestAutomation.Xunit
{
    public class SeleniumFacade
    { 
        public static IWebDriver CreateWebDriver(TargetBrowser seleniumConfig, string testName)
        {
            var testConfig = TestConfig.Instance;

            IWebDriver toReturn = null;

            if ((seleniumConfig & TargetBrowser.Chrome) == TargetBrowser.Chrome)
            {
                toReturn = new ChromeDriver();
            }
            if ((seleniumConfig & TargetBrowser.InternetExplorer) == TargetBrowser.InternetExplorer)
            {
                toReturn = new InternetExplorerDriver();
            }
            else if ((seleniumConfig & TargetBrowser.RemoteInternetExplorer) == TargetBrowser.RemoteInternetExplorer)
            {
                var remoteDriverConfig = testConfig.RemoteWebDriverConfig;

                DesiredCapabilities capability = DesiredCapabilities.InternetExplorer();

                if (remoteDriverConfig?.Capabiliities.ContainsKey(TargetBrowser.RemoteInternetExplorer) == true)
                {
                    var capabilities = remoteDriverConfig.Capabiliities[TargetBrowser.RemoteInternetExplorer];
                    foreach (var key in capabilities.Keys)
                    {
                        capability.SetCapability(key, capabilities[key]);
                    }

                    if ( !string.IsNullOrEmpty(testConfig.BuildNumber) )
                    {
                        capability.SetCapability("build", testConfig.BuildNumber);
                    }
                    
                    if ( !string.IsNullOrEmpty(testName) )
                    {
                        capability.SetCapability("name", testName);
                    }

                    capability.SetCapability("browserstack.user", remoteDriverConfig.User);
                    capability.SetCapability("browserstack.key", remoteDriverConfig.Password);

                    toReturn = new RemoteWebDriver(remoteDriverConfig.Url, capability);
                }
            }
            else if ((seleniumConfig & TargetBrowser.RemoteChrome) == TargetBrowser.RemoteChrome)
            {
                var remoteDriverConfig = testConfig.RemoteWebDriverConfig;

                DesiredCapabilities capability = DesiredCapabilities.Chrome();

                if (remoteDriverConfig?.Capabiliities.ContainsKey(TargetBrowser.RemoteChrome) == true)
                {
                    var capabilities = remoteDriverConfig.Capabiliities[TargetBrowser.RemoteChrome];
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
            }

            return toReturn;
        }
    }
}