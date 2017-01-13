using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Configuration;

using Microsoft.Extensions.Configuration;

using OlsonDigital.TestAutomation.IoC.Modules;
using OlsonDigital.TestAutomation.IoC;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// A test fixture for Autofac
    /// </summary>
    public class AutofacFixture 
    {
        private IContainer _container = ContainerFactory.Container;


        /// <summary>
        /// Creates the fixture
        /// </summary>
        public AutofacFixture()
        {

        }

        /// <summary>
        /// The root container
        /// </summary>
        protected IContainer Container => _container;

        /// <summary>
        /// Creates a scope specific for one test
        /// </summary>
        /// <param name="scopeConfig">Configuration for the container</param>
        /// <returns></returns>
        public virtual ILifetimeScope CreateTestScope(IDictionary<string, object> scopeConfig)
            => _container.BeginLifetimeScope((builder) => RegisterLifetimeScoped(builder, scopeConfig));

        /// <summary>
        /// Registers lifetime scoped objects
        /// </summary>
        /// <param name="containerBuilder">Builder for the container.</param>
        /// <param name="scopeConfig">Configuration for the container.</param>
        public virtual void RegisterLifetimeScoped(ContainerBuilder containerBuilder, IDictionary<string, object> scopeConfig)
        {

        }
    }
}
