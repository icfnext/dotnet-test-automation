using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

using Autofac;

using OlsonDigital.TestAutomation.Selenium;

namespace OlsonDigital.TestAutomation.IoC.Modules
{
    public class LocatorModule : Autofac.Module
    {
        private readonly string _assemblyName;

        public LocatorModule(string assemblyName)
        {
            _assemblyName = assemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var assembly = Assembly.Load(_assemblyName);

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<ILocator>();
        }
    }
}