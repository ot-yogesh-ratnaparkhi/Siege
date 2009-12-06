using System;
using System.Linq;
using Ninject;
using Ninject.Parameters;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.RegistrationExtensions.Ninject
{
    public class DecoratorUseCaseBinding<TService> : IDecoratorUseCaseBinding<TService>
    {
        private IKernel kernel;
        private IServiceLocatorAdapter locator;

        public DecoratorUseCaseBinding(IKernel kernel, IServiceLocatorAdapter locator)
        {
            this.kernel = kernel;
            this.locator = locator;
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
            kernel.Bind(useCase.GetBoundType()).ToSelf();
        }
    }
}
