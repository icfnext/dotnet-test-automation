using System;

using OlsonDigital.TestAutomation.Samples.Locators;
using OlsonDigital.TestAutomation.Selenium;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using Xunit;


namespace OlsonDigital.TestAutomation.Samples.Commands
{
    public class PerformSearch : ICommand
    {
        private readonly SearchControls _searchControls;
        private readonly IWebDriver _webDriver;

        public PerformSearch(IWebDriver webDriver, SearchControls searchControls)
        {
            _searchControls = searchControls;
            _webDriver = webDriver;
        }

        public void SearchAndValidateTitle(string searchText, string expectedPageTitle)
        {
            var searchBox = _webDriver.FindElement(_searchControls.SearchBox);
            if (searchBox != null )
            {
                searchBox.SendKeys(searchText);
                searchBox.SendKeys(Keys.Tab);

                var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(45))
                    .Until(ExpectedConditions.TitleIs(expectedPageTitle));

            }
            else
            {
                Assert.True(false, "Could not find the search box");
            }

        }
    }
}