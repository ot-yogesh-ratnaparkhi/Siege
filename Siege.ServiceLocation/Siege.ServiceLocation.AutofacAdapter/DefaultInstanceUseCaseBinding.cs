using System.Collections.Generic;
using Autofac;
using Autofac.Builder;

namespace Siege.ServiceLocation.AutofacAdapter
{
    public class DefaultInstanceUseCaseBinding<TService> : IDefaultInstanceUseCaseBinding<TService>
    {
        private IContainer container;
        private IServiceLocatorAdapter locator;

        public DefaultInstanceUseCaseBinding(IContainer container, IServiceLocatorAdapter locator)
        {
            this.container = container;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((DefaultInstanceUseCase<TService>)useCase);
        }

        private void Bind(DefaultInstanceUseCase<TService> useCase)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            var builder = new ContainerBuilder();

            if (typeof(TService) != useCase.GetBoundType())
            {
                if (!container.IsRegistered<IEnumerable<TService>>()) builder.RegisterCollection<TService>().As<IEnumerable<TService>>();
                builder.Register((c => factory.Build())).As<TService>().FactoryScoped().MemberOf<IEnumerable<TService>>();
            }

            builder.Register(useCase.GetBoundType());
            builder.Register(c => useCase.GetBinding());

            builder.Build(container);
        }
    }
}