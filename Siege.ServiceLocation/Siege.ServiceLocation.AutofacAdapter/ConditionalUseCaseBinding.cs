using System.Collections.Generic;
using Autofac;
using Autofac.Builder;

namespace Siege.ServiceLocation.AutofacAdapter
{
    public class ConditionalUseCaseBinding<TService> : IConditionalUseCaseBinding<TService>
    {
        private IContainer container;
        private IServiceLocatorAdapter locator;

        public ConditionalUseCaseBinding(IContainer container, IServiceLocatorAdapter locator)
        {
            this.container = container;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IConditionalUseCase<TService>)useCase);
        }

        private void Bind(IConditionalUseCase<TService> useCase)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            var builder = new ContainerBuilder();
            
            if(!container.IsRegistered<IEnumerable<TService>>()) builder.RegisterCollection<TService>().As<IEnumerable<TService>>();
            builder.Register((c => factory.Build())).As<TService>().FactoryScoped().MemberOf<IEnumerable<TService>>();
            builder.Register(useCase.GetBoundType()).FactoryScoped();

            builder.Build(container);
        }
    }
}