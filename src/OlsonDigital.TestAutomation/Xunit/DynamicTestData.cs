namespace OlsonDigital.TestAutomation.Xunit
{
    /// <summary>
    /// Represents data for a unit test read from a Json file
    /// </summary>
    public class DynamicTestData
    {
        private readonly string _testName;
        private readonly bool _shouldPass;


        /// <summary>
        /// Creates a new container for test data
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="shouldPass"></param>
        public DynamicTestData(string testName, bool shouldPass)
        {
            _shouldPass = shouldPass;
            _testName = testName;
        }

        /// <summary>
        /// The data from the Json file
        /// </summary>
        public dynamic Data { get; set; }

        /// <summary>
        /// Should the test expect to pass?
        /// </summary>
        public bool ShouldPass => _shouldPass;

        /// <summary>
        /// Returns the name of the test.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _testName;
        }
    }
}
