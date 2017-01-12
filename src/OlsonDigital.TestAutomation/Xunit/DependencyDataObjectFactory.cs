using System;
using System.Reflection;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// Creates a new Depedency Data Object which Mocks it's depedencies
    /// </summary>
    public class DependencyDataObjectFactory
    {
        private readonly Type _type;


        /// <summary>
        /// Creates a new Depedency Data Object
        /// </summary>
        /// <param name="type"></param>
        public DependencyDataObjectFactory(Type type)
        {
            _type = type;
        }


        /// <summary>
        /// Creates the Data Object
        /// </summary>
        /// <returns></returns>
        public object CreateDataObject()
        {
            var factoryType = typeof(DependentTypeMockFacade<>);
            var genericFactoryType = factoryType.MakeGenericType(_type);

            ConstructorInfo mockConstructor = genericFactoryType.GetConstructor(new Type[0]);

            var factory = mockConstructor.Invoke(new object[0]);

            return factory;
        }

    }
}