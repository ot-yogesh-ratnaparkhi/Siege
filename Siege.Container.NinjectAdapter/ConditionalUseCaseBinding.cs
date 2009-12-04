using Ninject;
using Siege.ServiceLocation;

namespace Siege.Container.NinjectAdapter
{
    public class ConditionalUseCaseBinding<TService> : IConditionalUseCaseBinding<TService>
    {
        private IKernel kernel;
        private IServiceLocatorAdapter locator;

        public ConditionalUseCaseBinding(IKernel kernel, IServiceLocatorAdapter locator)
        {
            this.kernel = kernel;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IConditionalUseCase<TService>)useCase);
        }

        private void Bind(IConditionalUseCase<TService> useCase)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            kernel.Bind<TService>().ToMethod(context => factory.Build(null));
            kernel.Bind(useCase.GetBoundType()).ToSelf();
        }
    }
}
