using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Configuration;

using Microsoft.Extensions.Configuration;

using OlsonDigital.TestAutomation.IoC.Modules;

namespace OlsonDigital.TestAutomation.Xunit
{
    public class AutofacFixture : IDisposable
    {
        private IContainer _container;

        public AutofacFixture()
        {

        }

        public void BuildAndRegisterConstainer(string[] assembliesToScan)
        {
            _container = BuildContainer(assembliesToScan);
        }

        internal IContainer BuildContainer(string[] assembliesToScan)
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile($"config/autofac.modules.json", true);  

            var module = new ConfigurationModule(configBuilder.Build());

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            foreach(string assembly in assembliesToScan)
            {
                builder.RegisterModule(new CommandModule(assembly));
                builder.RegisterModule(new LocatorModule(assembly));
            }

            var container = builder.Build();
            return container;
        }


        public IContainer Container => _container;


        public virtual void CustomRegistrations(ConfigurationBuilder configBuilder)
        {

        }

        public virtual ILifetimeScope CreateTestScope(IDictionary<string, object> scopeConfig)
        {
            return _container.BeginLifetimeScope((builder) => RegisterLifetimeScoped(builder, scopeConfig));
        }

        public virtual void RegisterLifetimeScoped(ContainerBuilder builderm, IDictionary<string, object> scopeConfig)
        {

        }

        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}
