using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Bindings
{
    public interface IActionUseCaseBinding<TService> : IUseCaseBinding
    {
    }

    public class ActionUseCaseBinding<TService> : IActionUseCaseBinding<TService> 
    {
        public void Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            
        }

        public void BindInstance(IInstanceUseCase useCase, IFactoryFetcher locator)
        {
        }
    }
}
