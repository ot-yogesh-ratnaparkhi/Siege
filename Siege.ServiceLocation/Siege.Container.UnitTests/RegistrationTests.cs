using System;
using System.Collections.Generic;
using NUnit.Framework;
using Siege.ServiceLocation.AOP;
using Siege.ServiceLocation.Extensions.ExtendedRegistrationSyntax;
using Siege.ServiceLocation.InternalStorage;
using Siege.ServiceLocation.RegistrationTemplates;
using Siege.ServiceLocation.RegistrationTemplates.Registration;
using Siege.ServiceLocation.Extensions.Conventions;
using Siege.ServiceLocation.Registrations;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Proxy_All_Types()
        {
            locator.Register(Using.Convention<ProxyConvention>());
        }
    }

    public class ProxyConvention : IConvention
    {
        public List<IRegistration> Build()
        {
            return new List<IRegistration> {Given<IRegistrationregistration>.Then<Proxyregistration>()};
        }
    }

    public class Proxyregistration : IRegistration, IRegistrationregistration
    {
        private readonly IRegistration registration;
        private readonly Type boundType;

        public Proxyregistration(SiegeProxy proxy, IRegistration registration)
        {
            this.registration = registration;
            boundType = proxy.WithServiceLocator().Create(registration.GetMappedToType());
        }

        public object GetMappedTo()
        {
            return registration.GetMappedTo();
        }

        public Type GetMappedToType()
        {
            return boundType;
        }

        public IRegistrationTemplate GetRegistrationTemplate()
        {
            return registration.GetRegistrationTemplate();
        }

        public Type GetMappedFromType()
        {
            return registration.GetMappedFromType();
        }

        public object ResolveWith(IResolutionStrategy strategy, IServiceLocatorStore accessor)
        {
            return registration.ResolveWith(strategy, accessor);
        }

        public bool IsValid(IServiceLocatorStore context)
        {
            return registration.IsValid(context);
        }

        Type IRegistrationregistration.GetregistrationBindingType()
        {
            return typeof(DefaultRegistrationTemplate);
        }

        bool IRegistrationregistration.IsValid(IServiceLocatorStore context)
        {
            return true;
        }
    }

    public interface IRegistrationregistration
    {
        Type GetregistrationBindingType();
        bool IsValid(IServiceLocatorStore context);
    }
}