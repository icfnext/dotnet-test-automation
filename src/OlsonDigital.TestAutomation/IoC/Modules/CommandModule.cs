using System.Reflection;

using Autofac;

using OlsonDigital.TestAutomation.Selenium;

namespace OlsonDigital.TestAutomation.IoC.Modules
{
    /// <summary>
    /// A Module to load Command objects
    /// </summary>
    public class CommandModule : Autofac.Module
    {
        private readonly string _assemblyName;

        /// <summary>
        /// Creates a new Command Module
        /// </summary>
        /// <param name="assemblyName"></param>
        public CommandModule(string assemblyName)
        {
            _assemblyName = assemblyName;
        }

        /// <summary>
        /// Loads classes that implement ICommand from the provided assembly
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var assembly = Assembly.Load(_assemblyName);

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<ICommand>();
        }
    }
}