using System.Reflection;

using Autofac;

using OlsonDigital.TestAutomation.Selenium;

namespace OlsonDigital.TestAutomation.IoC.Modules
{

    /// <summary>
    /// An Autofac Module for finding implementors of ILocator
    /// </summary>
    public class LocatorModule : Autofac.Module
    {
        private readonly string _assemblyName;

        /// <summary>
        /// Creates the module
        /// </summary>
        /// <param name="assemblyName"></param>
        public LocatorModule(string assemblyName)
        {
            _assemblyName = assemblyName;
        }


        /// <summary>
        /// Loads types into the container
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var assembly = Assembly.Load(_assemblyName);

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<ILocator>();
        }
    }
}