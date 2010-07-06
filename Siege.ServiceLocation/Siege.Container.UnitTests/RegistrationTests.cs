using System;
using System.Collections.Generic;
using NUnit.Framework;
using Siege.ServiceLocation.AOP;
using Siege.ServiceLocation.Bindings.Registration;
using Siege.ServiceLocation.Extensions.Conventions;
using Siege.ServiceLocation.Extensions.ExtendedSyntax;
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test, Ignore]
        public void Should_Proxy_All_Types()
        {
            locator.Register(Using.Convention<AOPConvention>());
        }
    }

    public class AOPConvention : IConvention
    {
        public List<IUseCase> Build()
        {
            return new List<IUseCase> {Given<IRegistrationUseCase>.Then<ProxyUseCase>()};
        }
    }

    public class ProxyUseCase : IUseCase, IRegistrationUseCase
    {
        private readonly IUseCase useCase;
        private Type boundType;

        public ProxyUseCase(SiegeProxy proxy, IUseCase useCase)
        {
            this.useCase = useCase;
            boundType = proxy.WithServiceLocator().Create(useCase.GetBoundType());
        }

        public object GetBinding()
        {
            return useCase.GetBinding();
        }

        public Type GetBoundType()
        {
            return boundType;
        }

        public Type GetUseCaseBindingType()
        {
            return useCase.GetUseCaseBindingType();
        }

        public Type GetBaseBindingType()
        {
            return useCase.GetBaseBindingType();
        }

        public object Resolve(IResolutionStrategy strategy, IServiceLocatorStore accessor)
        {
            return useCase.Resolve(strategy, accessor);
        }

        public bool IsValid(IServiceLocatorStore context)
        {
            return useCase.IsValid(context);
        }

        Type IRegistrationUseCase.GetUseCaseBindingType()
        {
            return typeof(DefaultRegistrationUseCaseBinding);
        }

        bool IRegistrationUseCase.IsValid(IServiceLocatorStore context)
        {
            return true;
        }
    }

    public interface IRegistrationUseCase
    {
        Type GetUseCaseBindingType();
        bool IsValid(IServiceLocatorStore context);
    }
}