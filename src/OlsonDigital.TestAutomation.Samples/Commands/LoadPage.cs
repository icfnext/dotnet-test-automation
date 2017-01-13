using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OlsonDigital.TestAutomation.Selenium;
using OpenQA.Selenium;

using Xunit;

namespace OlsonDigital.TestAutomation.Samples.Commands
{
    public class LoadPage : ICommand
    {
        private readonly IWebDriver _webDriver;

        public LoadPage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void LoadPageAndValidateTitle(string url, string expectedTitle)
        {
            _webDriver.Navigate().GoToUrl(url);

            Assert.Equal(expectedTitle, _webDriver.Title);
        }
    }
}