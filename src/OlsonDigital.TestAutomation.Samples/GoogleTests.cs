using Autofac;

using OlsonDigital.TestAutomation.Samples.Commands;
using OlsonDigital.TestAutomation.Xunit;

using Xunit;

namespace OlsonDigital.TestAutomation.Samples
{
    public class GoogleTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public GoogleTests(SeleniumFixture fixture)
        {
            _fixture = fixture;

            _fixture.BuildAndRegisterConstainer(new string[] { "OlsonDigital.TestAutomation.Samples" });
        }

        [Category("Sample Tests")]
        [Theory]
        [SeleniumAndJsonData(
            TargetBrowser.Chrome, 
            TestDataType.Object,
            "TestAssets/GoogleTests/TestSearch/Search-OlsonDigital")]
        public void TestSearch(TargetBrowser browser, DynamicTestData testData)
        {
            _fixture.RunTest(browser, "Test Search", (scope) =>
            {
                var data = testData.Data;

                var loadPage = scope.Resolve<LoadPage>();

                loadPage.LoadPageAndValidateTitle(data.Url, data.ExpectedSearchPageTitle);

                var performSearch = scope.Resolve<PerformSearch>();

                performSearch.SearchAndValidateTitle(data.SearchText, data.SearchResultsPageTitle);
            });
        }
    }
}