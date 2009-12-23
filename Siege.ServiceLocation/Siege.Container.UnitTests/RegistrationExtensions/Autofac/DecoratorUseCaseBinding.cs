using System;
using System.Linq;
using Autofac;
using Autofac.Builder;

namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions.Autofac
{
    public class DecoratorUseCaseBinding<TService> : IDecoratorUseCaseBinding<TService>
    {
        private IContainer container;

        public DecoratorUseCaseBinding(IContainer container)
        {
            this.container = container;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IDecoratorUseCase<TService>)useCase);
        }

        public object Resolve(Type typeToResolve, Type argumentType, object rootObject)
        {
            string parameterName = typeToResolve.GetConstructor(new[] { argumentType }).GetParameters().Where(parameter => parameter.ParameterType == argumentType).First().Name;

            return container.Resolve(typeToResolve, new NamedParameter(parameterName, rootObject));
        }

        private void Bind(IDecoratorUseCase<TService> useCase)
        {
            var builder = new ContainerBuilder();

            builder.Register(useCase.GetDecoratorType());

            builder.Build(container);
        }
    }
}
