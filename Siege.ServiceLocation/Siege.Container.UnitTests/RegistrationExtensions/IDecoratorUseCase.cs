using System;
using Siege.ServiceLocation;

namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions
{
    public interface IDecoratorUseCase : IUseCase
    {
        Type GetDecoratorType();
    }
    public interface IDecoratorUseCase<TService> : IDecoratorUseCase, IConditionalUseCase<TService> {}
}
