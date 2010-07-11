using System;
using System.Collections.Generic;
using NUnit.Framework;
using Siege.Requisitions.AOP;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.RegistrationTemplates;
using Siege.Requisitions.RegistrationTemplates.Registration;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Registrations;

namespace Siege.Requisitions.UnitTests
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
            return new List<IRegistration> { Given<IPreResolutionRegistration>.Then<ProxyRegistration>() };
        }
    }

    public class ProxyRegistration : IRegistration, IPreResolutionRegistration
    {
        private readonly IRegistration registration;
        private readonly Type boundType;

        public ProxyRegistration(SiegeProxy proxy, IRegistration registration)
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

        public object ResolveWith(IInstanceResolver resolver, IServiceLocatorStore context)
        {
            return registration.ResolveWith(resolver, context);
        }

        public bool IsValid(IServiceLocatorStore context)
        {
            return registration.IsValid(context);
        }

        IRegistrationTemplate IPreResolutionRegistration.GetRegistrationTemplate()
        {
            return new DefaultRegistrationTemplate();
        }

        bool IPreResolutionRegistration.IsValid(IServiceLocatorStore context)
        {
            return true;
        }
    }

    public interface IPreResolutionRegistration
    {
        IRegistrationTemplate GetRegistrationTemplate();
        bool IsValid(IServiceLocatorStore context);
    }
}