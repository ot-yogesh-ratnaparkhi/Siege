using Ninject;
using Siege.ServiceLocation;

namespace Siege.Container.NinjectAdapter
{
    public class DefaultInstanceUseCaseBinding<TService> : IDefaultInstanceUseCaseBinding<TService>
    {
        private IKernel kernel;
        private IServiceLocatorAdapter locator;

        public DefaultInstanceUseCaseBinding(IKernel kernel, IServiceLocatorAdapter locator)
        {
            this.kernel = kernel;
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

            if (typeof(TService) != useCase.GetBoundType()) kernel.Bind<TService>().ToMethod(context => factory.Build());
            kernel.Bind(useCase.GetBoundType()).ToConstant(useCase.GetBinding());
        }
    }
}
