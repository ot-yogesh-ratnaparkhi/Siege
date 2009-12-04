using System;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.RegistrationExtensions.Ninject
{
    public class DecoratorUseCaseBinding<TService> : IDecoratorUseCaseBinding<TService>
    {
        public void Bind(IUseCase useCase)
        {
            throw new NotImplementedException();
        }
    }
}
