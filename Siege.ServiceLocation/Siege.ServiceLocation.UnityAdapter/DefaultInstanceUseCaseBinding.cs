using System;
using Microsoft.Practices.Unity;

namespace Siege.ServiceLocation.UnityAdapter
{
    public class DefaultInstanceUseCaseBinding<TService> : IDefaultInstanceUseCaseBinding<TService>
    {
        private IUnityContainer container;
        private IServiceLocatorAdapter locator;

        public DefaultInstanceUseCaseBinding(IUnityContainer container, IServiceLocatorAdapter locator)
        {
            this.container = container;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((DefaultInstanceUseCase<TService>)useCase);
        }

        private void Bind(DefaultInstanceUseCase<TService> useCase)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            if (typeof(TService) != useCase.GetBoundType())
            {
                container.RegisterType<TService>(new TransientLifetimeManager(), new InjectionFactory(f => factory.Build()));
                container.RegisterType<TService>(Guid.NewGuid().ToString(), new TransientLifetimeManager(), new InjectionFactory(f => factory.Build()));
            }
            container.RegisterInstance(useCase.GetBoundType(), useCase.GetBinding(), new TransientLifetimeManager());
        }
    }
}
