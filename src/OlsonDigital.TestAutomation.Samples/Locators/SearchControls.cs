using OlsonDigital.TestAutomation.Selenium;
using OpenQA.Selenium;

namespace OlsonDigital.TestAutomation.Samples.Locators
{
    public class SearchControls : ILocator
    {
        public By SearchBox => By.Name("q");
    }
}