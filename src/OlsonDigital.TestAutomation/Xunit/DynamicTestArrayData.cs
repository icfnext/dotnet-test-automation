namespace OlsonDigital.TestAutomation.Xunit
{
    /// <summary>
    /// Represents an Array of Data from a Json Data Source
    /// </summary>
    public class DynamicTestArrayData
    {
        private readonly int _expectedResultCount;
        private readonly bool _shouldPass;
        private readonly string _testName;


        /// <summary>
        /// Creates a new Array Data container
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="shouldPass"></param>
        /// <param name="expectedResultCount"></param>
        public DynamicTestArrayData(string testName, bool shouldPass, int expectedResultCount)
        {
            _expectedResultCount = expectedResultCount;
            _shouldPass = shouldPass;
            _testName = testName;
        }


        /// <summary>
        /// The data from the Json
        /// </summary>
        public dynamic[] Data { get; set; }

        /// <summary>
        /// How many results should the test expect?
        /// </summary>
        public int ExpectedResultCount => _expectedResultCount;

        /// <summary>
        /// Should the test expect to pass?
        /// </summary>
        public bool ShouldPass => _shouldPass;

        /// <summary>
        /// Returns the name of the test case
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _testName;
        }
    }
}