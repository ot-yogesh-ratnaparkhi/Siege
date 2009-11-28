using Siege.ServiceLocation;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Siege.Container.StructureMapAdapter
{
    public class ConditionalUseCaseBinding<TService> : IConditionalUseCaseBinding<TService>
    {
        private StructureMap.Container container;
        private IServiceLocatorAdapter locator;

        public ConditionalUseCaseBinding(StructureMap.Container container, IServiceLocatorAdapter locator)
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
            Registry registry = new Registry();

            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            registry.ForRequestedType<TService>().CacheBy(InstanceScope.PerRequest).TheDefault.Is.ConstructedBy(context => factory.Build(null));
            registry.ForRequestedType(useCase.GetBoundType()).CacheBy(InstanceScope.PerRequest).AddType(useCase.GetBoundType());
            container.Configure(configure => configure.AddRegistry(registry));
        }
    }
}
