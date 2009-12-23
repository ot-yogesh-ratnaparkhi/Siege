using System;
using Microsoft.Practices.Unity;

namespace Siege.ServiceLocation.UnityAdapter
{
    public class DefaultUseCaseBinding<TService> : IDefaultUseCaseBinding<TService>
    {
        private IUnityContainer container;
        private IServiceLocatorAdapter locator;

        public DefaultUseCaseBinding(IUnityContainer container, IServiceLocatorAdapter locator)
        {
            this.container = container;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IDefaultUseCase<TService>)useCase);
        }

        private void Bind(IDefaultUseCase<TService> useCase)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            if (typeof(TService) != useCase.GetBoundType())
            {
                container.RegisterType<TService>(new TransientLifetimeManager(), new InjectionFactory(f => factory.Build()));
                container.RegisterType<TService>(Guid.NewGuid().ToString(), new TransientLifetimeManager(), new InjectionFactory(f => factory.Build()));
            }
            container.RegisterType(useCase.GetBoundType(), new TransientLifetimeManager());
        }
    }
}
