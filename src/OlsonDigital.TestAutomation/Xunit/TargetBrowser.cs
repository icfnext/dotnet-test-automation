using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlsonDigital.TestAutomation.Xunit
{
    [Flags]
    public enum TargetBrowser
    {
        Chrome = 1,

        InternetExplorer = 2,

        RemoteInternetExplorer = 4,

        RemoteChrome = 8
    }
}