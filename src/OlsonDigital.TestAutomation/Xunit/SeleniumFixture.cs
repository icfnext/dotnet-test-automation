using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Autofac.Core.Lifetime;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Net;
using System.IO;

namespace OlsonDigital.TestAutomation.Xunit
{
    public class SeleniumFixture : AutofacFixture
    {

        public ILifetimeScope CreateTestScope(TargetBrowser targetBrowser, string testName)
            => CreateTestScope(targetBrowser, testName, new ExpandoObject() as IDictionary<string, object>);


        public ILifetimeScope CreateTestScope(TargetBrowser targetBrowser, string testName, IDictionary<string, object> scopeConfig)
        {
            scopeConfig["TargetBrowser"] = targetBrowser;
            scopeConfig["TestName"] = testName;

            var scope =  base.CreateTestScope(scopeConfig);

            return scope;
        }

        internal void OnScopeEnding(object sender, LifetimeScopeEndingEventArgs e)
        {
            var scope = sender as ILifetimeScope;

            if (scope != null && scope.IsRegistered<IWebDriver>())
            {
                var wd = scope.Resolve<IWebDriver>();

                wd?.Quit();
            }
        }

        public override void RegisterLifetimeScoped(ContainerBuilder builder, IDictionary<string, object> scopeConfig)
        {
            base.RegisterLifetimeScoped(builder, scopeConfig);

            RegisterWebDriver(builder, scopeConfig);
        }

        internal void RegisterWebDriver(ContainerBuilder builder, IDictionary<string, object> scopeConfig)
        {
            if ( scopeConfig.ContainsKey("TargetBrowser") )
            {
                var targetBrowser = (TargetBrowser)scopeConfig["TargetBrowser"];
                var testName = scopeConfig.ContainsKey("TestName") ? (string) scopeConfig["TestName"] : null;

                var webDriver = SeleniumFacade.CreateWebDriver(targetBrowser, testName);

                builder.Register<IWebDriver>(wd => webDriver);
            }
        }


        public virtual void RunTest(TargetBrowser targetBrowser, string testName, Action<ILifetimeScope> test)
        {
            using (var scope = CreateTestScope(targetBrowser, testName))
            {
                var driver = scope.Resolve<IWebDriver>();
                var sessionId = driver as RemoteWebDriver != null ? (driver as RemoteWebDriver).SessionId.ToString() : string.Empty;

                try
                {
                    test?.Invoke(scope);

                    //ReportPassFailToBrowserstack(scope, sessionId, true, string.Empty);
                }
                catch (Exception e)
                {
                    //ReportPassFailToBrowserstack(scope, sessionId, false, e.Message);

                    throw;
                }
                finally
                {
                    driver?.Quit();
                }
            }
        }

        internal void ReportPassFailToBrowserstack(ILifetimeScope scope, string sessionId, bool passed, string statusText)
        {
            TestConfig testConfig = TestConfig.Instance;

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