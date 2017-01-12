using System;

using Xunit.Sdk;

namespace OlsonDigital.TestAutomation.Xunit
{
    /// <summary>
    /// Puts a Category Trait on a unit test
    /// </summary>
    [TraitDiscoverer("OlsonDigital.TestAutomation.Xunit.CategoryDiscoverer", "OlsonDigital.TestAutomation")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CategoryAttribute : Attribute, ITraitAttribute
    {

        /// <summary>
        /// Creates a new Category Attriubute
        /// </summary>
        /// <param name="name"></param>
        public CategoryAttribute(string name) { }

    }
}