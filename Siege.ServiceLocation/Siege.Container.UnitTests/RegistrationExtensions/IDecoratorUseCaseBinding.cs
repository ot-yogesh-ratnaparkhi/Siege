using System;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.RegistrationExtensions
{
    public interface IDecoratorUseCaseBinding : IUseCaseBinding
    {
        object Resolve(Type typeToResolve, Type argumentType, object rootObject);
    }

    public interface IDecoratorUseCaseBinding<TService> : IDecoratorUseCaseBinding
    {
        
    }
}