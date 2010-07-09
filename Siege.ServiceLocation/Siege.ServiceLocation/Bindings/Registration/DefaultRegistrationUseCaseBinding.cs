using System;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Bindings.Registration
{
    public class DefaultRegistrationUseCaseBinding : IUseCaseBinding
    {
        public void Bind(IServiceLocatorAdapter adapter, IUseCase useCase, IFactoryFetcher locator)
        {
            throw new NotImplementedException();
        }
    }
}