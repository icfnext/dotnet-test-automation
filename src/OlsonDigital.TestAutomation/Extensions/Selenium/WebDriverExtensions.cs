using System.Linq;
using OpenQA.Selenium;

namespace OlsonDigital.TestAutomation.Extensions.Selenium
{
    /// <summary>
    /// Extensions for the Selenium WebDriver
    /// </summary>
    public static class WebDriverExtensions
    {

        /// <summary>
        /// Checks to see if the provided element has the provided class
        /// </summary>
        /// <param name="element"></param>
        /// <param name="expectedClass"></param>
        /// <returns></returns>
        public static bool HasCssClass(this IWebElement element, string expectedClass)
        {
            var classes = element.GetAttribute("class");

            return classes?.Split(' ').Contains(expectedClass) ?? false;
        }
    }
}