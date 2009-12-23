using System;
using System.Linq;
using Microsoft.Practices.Unity;

namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions.Unity
{
    public class DecoratorUseCaseBinding<TService> : IDecoratorUseCaseBinding<TService>
    {
        private IUnityContainer container;
        private IServiceLocatorAdapter locator;

        public DecoratorUseCaseBinding(IUnityContainer container, IServiceLocatorAdapter locator)
        {
            this.container = container;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IDecoratorUseCase<TService>)useCase);
        }

        public object Resolve(Type typeToResolve, Type argumentType, object rootObject)
        {
            string parameterName = typeToResolve.GetConstructor(new[] { argumentType }).GetParameters().Where(parameter => parameter.ParameterType == argumentType).First().Name;

            return container.Resolve(typeToResolve, new ParameterOverride(parameterName, rootObject));
        }

        private void Bind(IDecoratorUseCase<TService> useCase)
        {
            container.RegisterType(useCase.GetBoundType());
            container.RegisterType(useCase.GetDecoratorType());
        }
    }
}
