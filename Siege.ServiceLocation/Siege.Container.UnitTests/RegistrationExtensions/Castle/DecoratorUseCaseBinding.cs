using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;

namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions.Castle
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
            string parameterName = typeToResolve.GetConstructor(new[] { argumentType }).GetParameters().Where(parameter => parameter.ParameterType == argumentType).First().Name;

            Dictionary<string, object> dictionary = new Dictionary<string, object> {{parameterName, rootObject}};
            return kernel.Resolve(typeToResolve, dictionary);
        }

        private void Bind(IDecoratorUseCase<TService> useCase)
        {
            kernel.Register(Component.For(useCase.GetDecoratorType()).ImplementedBy(useCase.GetDecoratorType()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }
    }
}
