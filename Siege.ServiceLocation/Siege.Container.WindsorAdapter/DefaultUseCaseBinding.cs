using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.SeviceLocation.WindsorAdapter
{
    public class DefaultUseCaseBinding<TService> : IDefaultUseCaseBinding<TService>
    {
        private IKernel kernel;
        private IServiceLocatorAdapter locator;

        public DefaultUseCaseBinding(IKernel kernel, IServiceLocatorAdapter locator)
        {
            this.kernel = kernel;
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

            if (typeof(TService) != useCase.GetBoundType()) kernel.Register(Component.For<TService>().UsingFactoryMethod(() => factory.Build()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
            kernel.Register(Component.For(useCase.GetBoundType()).ImplementedBy(useCase.GetBoundType()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }
    }
}