using System;
using System.Collections.Generic;

using OlsonDigital.TestAutomation.Mocks;

namespace OlsonDigital.TestAutomation.Utils
{
    /// <summary>
    /// Provides some utility methods for dealing with Types in unit tests
    /// </summary>
    public class TypeUtils
    {

        /// <summary>
        /// Provides a shallow copy of all of the dynamic objects in the array to the generic type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T[] CreateFromDynamic<T>(dynamic[] data) where T: new()
        {
            var toReturn = new List<T>();

            if ( data != null )
            {
                foreach (var d in data)
                {
                    toReturn.Add(CreateFromDynamic<T>(d));
                }
            }

            return toReturn.ToArray();
        }



        /// <summary>
        /// Performs a shallow copy from the provided dynamic object to the generic type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T CreateFromDynamic<T>(dynamic data) where T : new()
        {
            var toReturn = new T();
            var dictionary = data as IDictionary<string, object>;

            foreach(var prop in typeof(T).GetProperties())
            {
                if ( dictionary.ContainsKey(prop.Name))
                {
                    var value = dictionary[prop.Name];
                    var propType = prop.PropertyType;
                    if(value != null && value.GetType().IsArray && propType.IsArray)
                    {
                        var values = (object[])value;
                        var elementType = propType.GetElementType();

                        var targetArray = Array.CreateInstance(elementType, values.Length);
                        for(int i=0; i< targetArray.Length; i++)
                        {
                            var targetType = Convert.ChangeType(values[i], elementType);
                            targetArray.SetValue(targetType, i);
                        }

                        var castedArray = Convert.ChangeType(targetArray, propType);
                        prop.SetValue(toReturn, castedArray);
                    }
                    else
                    {
                        prop.SetValue(toReturn, value);
                    }
                }
            }

            return toReturn;
        }


        /// <summary>
        /// Maps a dynamic array into a new TestObjectResult for Mocking the Entity Framework
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TestableObjectResult<T> MapTestableResult<T>(dynamic data) where T : new()
        {
            dynamic[] dataArray = new dynamic[] { data };

            if ( data != null && data.GetType().IsArray )
            {
                dataArray = data;
            }

            var result = new TestableObjectResult<T>(
                CreateFromDynamic<T>(dataArray)
                );

            return result;
        }
    }
}
