using Xunit;

using OlsonDigital.TestAutomation.Xunit;

namespace OlsonDigital.TestAutomation.Tests.Xunit
{
    public class DependencyDataObjectFactoryTests
    {
        [Category("Test Extensions")]
        [Fact]
        public void TestCreateDataObject()
        {
            var factory = new DependencyDataObjectFactory(typeof(object));

            Assert.Equal(typeof(DependentTypeMockFacade<object>), factory.CreateDataObject()?.GetType());
        }

    }
}