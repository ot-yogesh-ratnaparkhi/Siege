using Microsoft.Practices.Unity;

namespace Siege.ServiceLocation.UnityAdapter
{
    public class KeyBasedUseCaseBinding<TService> : IKeyBasedUseCaseBinding<TService>
    {
        private IUnityContainer container;

        public KeyBasedUseCaseBinding(IUnityContainer container)
        {
            this.container = container;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IKeyBasedUseCase<TService>)useCase);
        }

        public void Bind(IKeyBasedUseCase<TService> useCase)
        {
            container.RegisterType(useCase.GetBaseBindingType(), useCase.GetBoundType(), useCase.Key, new TransientLifetimeManager());
        }
    }
}
