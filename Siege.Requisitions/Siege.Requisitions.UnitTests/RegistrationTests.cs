using System;
using System.Collections.Generic;
using NUnit.Framework;
using Siege.Arsenal;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.Registrations.Meta;
using Siege.Requisitions.Registrations.Named;
using Siege.Requisitions.Registrations.Stores;
using Siege.Requisitions.RegistrationTemplates;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Resolution.Pipeline;
using Siege.Requisitions.UnitTests.AOP;

namespace Siege.Requisitions.UnitTests
{
    public abstract partial class ServiceLocatorTests
    {
        [Test]
        public virtual void ShouldProxyAllTypes()
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
        public Action<IServiceLocator> Build()
        {
            return serviceLocator => serviceLocator.Register(new List<IRegistration>
                       {
                           Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>(),
                           Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>(),
                           Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>()
                       });
        }
    }

    public class ProxyConvention : IConvention
    {
        public Action<IServiceLocator> Build()
        {
            return serviceLocator => serviceLocator.Register(new List<IRegistration>
                       {
                           Given<SiegeProxy>.Then<SiegeProxy>(),
                           Given<IMetaRegistration>.Then<ProxyRegistration>()
                       });
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

        public IRegistrationStore GetRegistrationStore()
        {
            return registration.GetRegistrationStore();
        }

        public IRegistrationTemplate GetRegistrationTemplate()
        {
            return registration.GetRegistrationTemplate();
        }

        public Type GetMappedFromType()
        {
            return registration.GetMappedFromType();
        }

        public object ResolveWith(IInstanceResolver resolver, IServiceLocatorStore context, PostResolutionPipeline pipeline)
        {
            return registration.ResolveWith(resolver, context, pipeline);
        }

        public bool IsValid(IInstanceResolver locator, IServiceLocatorStore context)
        {
            return registration.IsValid(locator, context);
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
            get
            {
                if(registration is INamedRegistration) return ((INamedRegistration) registration).Key;

                return null;
            }
        }

        public bool Equals(IRegistration other)
        {
            return this.registration.Equals(other);
        }
    }
}