using Ninject;
using Siege.ServiceLocation;

namespace Siege.Container.NinjectAdapter
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

            if (typeof(TService) != useCase.GetBoundType()) kernel.Bind<TService>().ToMethod(context => factory.Build());
            kernel.Bind(useCase.GetBoundType()).ToSelf();
        }
    }
}
