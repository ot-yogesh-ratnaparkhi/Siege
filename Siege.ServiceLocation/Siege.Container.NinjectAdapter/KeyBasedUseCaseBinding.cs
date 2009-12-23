using Ninject;

namespace Siege.ServiceLocation.NinjectAdapter
{
    public class KeyBasedUseCaseBinding<TService> : IKeyBasedUseCaseBinding<TService>
    {
        private IKernel kernel;

        public KeyBasedUseCaseBinding(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IKeyBasedUseCase<TService>)useCase);
        }

        public void Bind(IKeyBasedUseCase<TService> useCase)
        {
            kernel.Bind<TService>().To(useCase.GetBoundType()).Named(useCase.Key);
        }
    }
}