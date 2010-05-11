using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using Siege.ServiceLocation.Bindings;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.AutofacAdapter
{
    public class OpenGenericUseCaseBinding : IOpenGenericUseCaseBinding
    {
        private IContainer container;

        public OpenGenericUseCaseBinding(IContainer container)
        {
            this.container = container;
        }

        public void Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            var factory = (Factory<object>) locator.GetFactory<object>();
            factory.AddCase(useCase);

            var builder = new ContainerBuilder();

            //if (typeof(TService) != useCase.GetBoundType())
            //{
            //    if (!container.IsRegistered<IEnumerable<TService>>()) builder.RegisterCollection<TService>().As<IEnumerable<TService>>();
            //    builder.Register((c => factory.Build())).As<TService>().FactoryScoped().MemberOf<IEnumerable<TService>>();
            //}

            //Type enumerableType = typeof (IEnumerable<>).MakeGenericType(useCase.GetBaseBindingType());
            //Type concreteEnumerableType = typeof (IEnumerable<>).MakeGenericType(useCase.GetBoundType());
            //if (!container.IsRegistered(enumerableType)) builder.RegisterCollection(concreteEnumerableType).As(enumerableType);

            builder.RegisterGeneric(useCase.GetBoundType()).As(useCase.GetBaseBindingType()).FactoryScoped();

            builder.Build(container);
        }
    }
}