using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace OlsonDigital.TestAutomation.Mocks
{
    /// <summary>
    /// A subclass of ObjectResult to use in unit tests
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TestableObjectResult<T> : ObjectResult<T>
    {
        private readonly T[] _sourceData = new T[0];


        /// <summary>
        /// A default constructor
        /// </summary>
        public TestableObjectResult()
        {

        }

        /// <summary>
        /// Takes an array of data to return later
        /// </summary>
        /// <param name="toCopy"></param>
        public TestableObjectResult(T[] toCopy)
        {
            _sourceData = toCopy;
        }


        /// <summary>
        /// Override GetEnumerator so we can return the data that was passed to us.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<T> GetEnumerator()
        {
            return new List<T>(_sourceData).GetEnumerator();
        }
    }
}