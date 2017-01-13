using System;

namespace OlsonDigital.TestAutomation.Xunit
{
    /// <summary>
    /// Target Browsers for Selenium Tests
    /// </summary>
    [Flags]
    public enum TargetBrowser
    {
        /// <summary>
        /// A local Chrome Web Driver
        /// </summary>
        Chrome = 1,

        /// <summary>
        /// A local IE Web Driver
        /// </summary>
        InternetExplorer = 2,

        /// <summary>
        /// A Remote IE Web Driver
        /// </summary>
        RemoteInternetExplorer = 4,

        /// <summary>
        /// A Remote Chrome Web Driver
        /// </summary>
        RemoteChrome = 8
    }
}