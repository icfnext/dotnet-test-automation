using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

using Autofac;
using Autofac.Core.Lifetime;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Net;
using System.IO;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// A Test Fixture for Selenium Tests
    /// </summary>
    public class SeleniumFixture : AutofacFixture
    {
        /// <summary>
        /// Creates a Test Scope for the provided browser
        /// </summary>
        /// <param name="targetBrowser"></param>
        /// <param name="testName"></param>
        /// <returns></returns>
        public ILifetimeScope CreateTestScope(TargetBrowser targetBrowser, string testName)
            => CreateTestScope(targetBrowser, testName, new ExpandoObject() as IDictionary<string, object>);


        /// <summary>
        /// Creates a Test Scope for the provided browser and config
        /// </summary>
        /// <param name="targetBrowser"></param>
        /// <param name="testName"></param>
        /// <param name="scopeConfig"></param>
        /// <returns></returns>
        public ILifetimeScope CreateTestScope(TargetBrowser targetBrowser, string testName, IDictionary<string, object> scopeConfig)
        {
            scopeConfig["TargetBrowser"] = targetBrowser;
            scopeConfig["TestName"] = testName;

            var scope =  base.CreateTestScope(scopeConfig);

            return scope;
        }

        /// <summary>
        /// Registers Lifetime scoped objects.  In this case it registers the IWebDriver
        /// </summary>
        /// <param name="builder">The builder for the container.</param>
        /// <param name="scopeConfig">The current scope config</param>
        public override void RegisterLifetimeScoped(ContainerBuilder builder, IDictionary<string, object> scopeConfig)
        {
            base.RegisterLifetimeScoped(builder, scopeConfig);

            RegisterWebDriver(builder, scopeConfig);
        }


        /// <summary>
        /// Registers a Web Driver for the scope
        /// </summary>
        /// <param name="builder">The container builder</param>
        /// <param name="scopeConfig">The current scope config</param>
        internal void RegisterWebDriver(ContainerBuilder builder, IDictionary<string, object> scopeConfig)
        {
            if ( scopeConfig.ContainsKey("TargetBrowser") )
            {
                var targetBrowser = (TargetBrowser)scopeConfig["TargetBrowser"];
                var testName = scopeConfig.ContainsKey("TestName") ? (string) scopeConfig["TestName"] : null;
                var testConfig = Container.Resolve<TestConfig>();

                var webDriver = WebDriverFactory.CreateWebDriver(testConfig, targetBrowser, testName);

                builder.Register<IWebDriver>(wd => webDriver);
            }
        }

        /// <summary>
        /// Sets up a scope for running a test
        /// </summary>
        /// <param name="targetBrowser">The target browser.</param>
        /// <param name="testName">The name of the test.</param>
        /// <param name="test">An Action that is executed for the test.</param>
        public virtual void RunTest(TargetBrowser targetBrowser, string testName, Action<ILifetimeScope> test)
        {
            using (var scope = CreateTestScope(targetBrowser, testName))
            {
                var driver = scope.Resolve<IWebDriver>();
                var sessionId = driver as RemoteWebDriver != null ? (driver as RemoteWebDriver).SessionId.ToString() : string.Empty;
                var testConfig = scope.Resolve<TestConfig>();

                try
                {
                    test?.Invoke(scope);

                    if ( testConfig?.RemoteWebDriverConfig?.Enabled == true)
                    {
                        ReportPassFailToBrowserstack(testConfig, sessionId, true, string.Empty);
                    }
                }
                catch (Exception e)
                {
                    if (testConfig?.RemoteWebDriverConfig?.Enabled == true)
                    {
                        ReportPassFailToBrowserstack(testConfig, sessionId, false, e.Message);
                    }

                    throw;
                }
                finally
                {
                    driver?.Quit();
                }
            }
        }


        /// <summary>
        /// Reports success / fail to Browserstack
        /// </summary>
        /// <param name="testConfig">The current test config</param>
        /// <param name="sessionId">The RemoteDriver Session Id</param>
        /// <param name="passed">Did the test pass or fail?</param>
        /// <param name="statusText">The The status of the test, usually an error message.</param>
        internal void ReportPassFailToBrowserstack(TestConfig testConfig, string sessionId, bool passed, string statusText)
        {
            dynamic request = new ExpandoObject();
            request.status = passed ? "completed" : "error";
            request.reason = statusText;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            byte[] requestData = Encoding.UTF8.GetBytes(json);

            Uri myUri = new Uri(string.Format($"https://www.browserstack.com/automate/sessions/{sessionId}.json"));

            WebRequest myWebRequest = HttpWebRequest.Create(myUri);
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myWebRequest.ContentType = "application/json";
            myWebRequest.Method = "PUT";
            myWebRequest.ContentLength = requestData.Length;
            using (Stream st = myWebRequest.GetRequestStream()) st.Write(requestData, 0, requestData.Length);

            NetworkCredential myNetworkCredential = new NetworkCredential(
                testConfig.RemoteWebDriverConfig.User,
                testConfig.RemoteWebDriverConfig.Password);

            CredentialCache myCredentialCache = new CredentialCache();
            myCredentialCache.Add(myUri, "Basic", myNetworkCredential);
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Credentials = myCredentialCache;


            //try-finally?
            myWebRequest.GetResponse().Close();
        }
    }
}