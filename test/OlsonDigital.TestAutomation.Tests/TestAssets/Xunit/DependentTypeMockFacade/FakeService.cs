
namespace OlsonDigital.TestAutomation.Tests.TestAssets.Xunit.DependentTypeMockFacade
{
    public class FakeService
    {
        public FakeService(IFakeRepositoryOne repoOne, IFakeRepositoryTwo repoTwo)
        {

        }
    }

    public interface IFakeRepositoryOne
    {

    }

    public interface IFakeRepositoryTwo
    {

    }

    public interface IFakeRepositoryThree
    {

    }

}
