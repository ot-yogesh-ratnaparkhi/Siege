using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Siege.ServiceLocation.StructureMapAdapter
{
    public class DefaultUseCaseBinding<TService> : IDefaultUseCaseBinding<TService>
    {
        private StructureMap.Container container;
        private IServiceLocatorAdapter locator;

        public DefaultUseCaseBinding(StructureMap.Container container, IServiceLocatorAdapter locator)
        {
            this.container = container;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IDefaultUseCase<TService>)useCase);
        }

        private void Bind(IDefaultUseCase<TService> useCase)
        {
            Registry registry = new Registry();

            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            if (typeof(TService) != useCase.GetBoundType()) registry.ForRequestedType<TService>().CacheBy(InstanceScope.PerRequest).TheDefault.Is.ConstructedBy(context => factory.Build());
            registry.ForRequestedType(useCase.GetBoundType()).CacheBy(InstanceScope.PerRequest).AddType(useCase.GetBoundType());
            container.Configure(configure => configure.AddRegistry(registry));
        }
    }
}