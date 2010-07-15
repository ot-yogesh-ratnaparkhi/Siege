using System;
using System.Collections.Generic;
using NUnit.Framework;
using Siege.Requisitions.AOP;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.Registrations.Meta;
using Siege.Requisitions.Registrations.Named;
using Siege.Requisitions.RegistrationTemplates;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.UnitTests.AOP;

namespace Siege.Requisitions.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public virtual void Should_Proxy_All_Types()
        {
            locator.Register(Using.Convention<SampleProxyAttributeConvention>());
            locator.Register(Using.Convention<ProxyConvention>());
            locator.Register(Given<TestType2>.Then<TestType2>());

            var testType2 = locator.GetInstance<TestType2>();
            Assert.AreEqual("lolarg1", testType2.Test("arg1", "arg2"));
            Assert.AreEqual(3, Counter.Count);

            Counter.Count = 0;
        }
    }

    public class SampleProxyAttributeConvention : IConvention
    {
        public List<IRegistration> Build()
        {
            return new List<IRegistration>
                       {
                           Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>(),
                           Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>(),
                           Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>()
                       };
        }
    }

    public class ProxyConvention : IConvention
    {
        public List<IRegistration> Build()
        {
            return new List<IRegistration>
                       {
                           Given<SiegeProxy>.Then<SiegeProxy>(),
                           Given<IMetaRegistration>.Then<ProxyRegistration>()
                       };
        }
    }

    public class ProxyRegistration : IMetaRegistration
    {
        private readonly SiegeProxy proxy;
        private IRegistration registration;

        public ProxyRegistration(SiegeProxy proxy)
        {
            this.proxy = proxy;
        }

        public void MapsTo(object implementationType)
        {
            this.registration.MapsTo(implementationType);
        }

        public object GetMappedTo()
        {
            return registration.GetMappedTo();
        }

        public Type GetMappedToType()
        {
            return (Type)registration.GetMappedTo();
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

        public void ChainTo(IRegistration registration)
        {
            this.registration = registration;
            this.registration.MapsTo(proxy.WithServiceLocator().Create(registration.GetMappedToType()));
        }

        public bool IsValid(IRegistration registration)
        {
            return true;
        }

        public string Key
        {
            get { return ((INamedRegistration) registration).Key; }
        }
    }
}