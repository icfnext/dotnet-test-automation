using System;
using System.Collections.Generic;
using System.Reflection;

using Moq;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// Simplifies mocking objects that result on DI framworks
    /// </summary>
    /// <typeparam name="T">The type that will be created</typeparam>
    public class DependentTypeMockFacade<T> where T : class
    {

        private readonly ConstructorInfo _typeConstructor;
        private readonly IDictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();


        /// <summary>
        /// Creates the facade
        /// </summary>
        public DependentTypeMockFacade()
        {
            var constructors = typeof(T).GetConstructors();

            // Lets just assume on the first constructor
            _typeConstructor = constructors[0];

            RegisterDependentMocks();
        }


        /// <summary>
        /// Creates a new instance of the type, defining mocks for each dependency.
        /// 
        /// Note: Need to call GetMock for anyting that you want to set up before calling this
        /// </summary>
        public T Instance
        {
            get
            {
                List<object> constructorParams = new List<object>();

                foreach (var param in _typeConstructor.GetParameters())
                {
                    var paramType = param.ParameterType;
                    var paramMock = _mocks[paramType];

                    if (paramMock != null)
                    {
                        constructorParams.Add(paramMock.Object);
                    }
                    else
                    {
                        constructorParams.Add(null);
                    }
                }

                return _typeConstructor.Invoke(constructorParams.ToArray()) as T;
            }
        }


        /// <summary>
        /// Gets a Mock for the provided type
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <returns></returns>
        public Mock<D> GetMock<D>() where D : class
        {
            return _mocks.ContainsKey(typeof(D)) ?_mocks[typeof(D)] as Mock<D> : null;
        }



        internal void RegisterDependentMocks()
        {
            var mock = typeof(Mock<>);

            foreach (var param in _typeConstructor.GetParameters())
            {
                var paramType = param.ParameterType;
                var genericMock = mock.MakeGenericType(paramType);

                ConstructorInfo mockConstructor = genericMock.GetConstructor(new Type[0]);

                var o = mockConstructor.Invoke(new object[0]) as Mock;

                _mocks.Add(paramType, o);
            }
        }

    }
}