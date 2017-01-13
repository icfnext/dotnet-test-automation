using System;

using Autofac;
using Autofac.Configuration;

using Microsoft.Extensions.Configuration;
using OlsonDigital.TestAutomation.IoC.Modules;
using OlsonDigital.TestAutomation.Xunit;

namespace OlsonDigital.TestAutomation.IoC
{
    /// <summary>
    /// A factory to create a root Container 
    /// </summary>
    public static class ContainerFactory
    {
        private static readonly Lazy<IContainer> _container = new Lazy<IContainer>(() =>
        {
            return BuildContainer();
        });

        private static readonly TestConfig _testConfig;

        static ContainerFactory()
        {
            _testConfig = CreateTestConfig();
        }

        /// <summary>
        /// Builds a new container
        /// </summary>
        /// <returns></returns>
        internal static IContainer BuildContainer()
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile($"config/autofac.modules.json", true);

            var module = new ConfigurationModule(configBuilder.Build());

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            foreach (string assembly in _testConfig?.TargetAssemblies)
            {
                builder.RegisterModule(new CommandModule(assembly));
                builder.RegisterModule(new LocatorModule(assembly));
            }

            builder.RegisterInstance(_testConfig);

            var container = builder.Build();
            return container;
        }

        /// <summary>
        /// Loads the test config from a test-config file
        /// </summary>
        /// <returns></returns>
        static TestConfig CreateTestConfig()
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile("test-config.json", false, true);
            configBuilder.AddEnvironmentVariables();

            var configRoot = configBuilder.Build();

            return new TestConfig(configRoot);
        }

        /// <summary>
        /// Returns the root container object
        /// </summary>
        public static IContainer Container => _container.Value;
    }
}