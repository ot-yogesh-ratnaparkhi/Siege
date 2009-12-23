using System;
using System.Linq;
using Ninject;
using Ninject.Parameters;

namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions.Ninject
{
    public class DecoratorUseCaseBinding<TService> : IDecoratorUseCaseBinding<TService>
    {
        private IKernel kernel;

        public DecoratorUseCaseBinding(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IDecoratorUseCase<TService>)useCase);
        }

        public object Resolve(Type typeToResolve, Type argumentType, object rootObject)
        {
            string parameterName = typeToResolve.GetConstructor(new[] {argumentType}).GetParameters().Where(parameter => parameter.ParameterType == argumentType).First().Name;

            return kernel.Get(typeToResolve, new ConstructorArgument(parameterName, rootObject));
        }

        private void Bind(IDecoratorUseCase<TService> useCase)
        {
            kernel.Bind(useCase.GetDecoratorType()).ToSelf();
        }
    }
}
