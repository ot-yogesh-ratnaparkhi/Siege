using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Bindings
{
    public interface IInstanceUseCaseBinding
    {
        void BindInstance(IInstanceUseCase useCase, IFactoryFetcher locator);
    }
}