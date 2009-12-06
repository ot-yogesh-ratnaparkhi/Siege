using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.RegistrationExtensions.Castle
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
            string parameterName = typeToResolve.GetConstructor(new[] { argumentType }).GetParameters().Where(parameter => parameter.ParameterType == argumentType).First().Name;

            Dictionary<string, object> dictionary = new Dictionary<string, object> {{parameterName, rootObject}};
            return kernel.Resolve(typeToResolve, dictionary);
        }

        private void Bind(IDecoratorUseCase<TService> useCase)
        {
            kernel.Register(Component.For(useCase.GetBoundType()).ImplementedBy(useCase.GetBoundType()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
            kernel.Register(Component.For(useCase.GetDecoratorType()).ImplementedBy(useCase.GetDecoratorType()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }
    }
}
