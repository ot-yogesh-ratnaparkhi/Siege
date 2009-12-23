using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.SeviceLocation.WindsorAdapter
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
            kernel.Register(Component.For(useCase.GetBoundType()).ImplementedBy(useCase.GetBoundType()).Named(useCase.Key));
        }
    }
}