
using Moq;
using Xunit;

using OlsonDigital.TestAutomation.Xunit;
using OlsonDigital.TestAutomation.Tests.TestAssets.Xunit.DependentTypeMockFacade;

namespace OlsonDigital.TestAutomation.Tests.Xunit
{
    public class DependentTypeMockFacadeTests
    {
        [Category("Test Extensions")]
        [Fact]
        public void TestFacade()
        {
            var facade = new DependentTypeMockFacade<FakeService>();

            Assert.Equal(typeof(Mock<IFakeRepositoryOne>), facade.GetMock<IFakeRepositoryOne>().GetType());
            Assert.Equal(typeof(Mock<IFakeRepositoryTwo>), facade.GetMock<IFakeRepositoryTwo>().GetType());
            Assert.Null(facade.GetMock<IFakeRepositoryThree>());

            var instance = facade.Instance;

            Assert.NotNull(instance);
            Assert.Equal(typeof(FakeService), instance?.GetType());
        }
    }
}