using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Siege.ServiceLocation.StructureMapAdapter
{
    public class KeyBasedUseCaseBinding<TService> : IKeyBasedUseCaseBinding<TService>
    {
        private StructureMap.Container container;

        public KeyBasedUseCaseBinding(StructureMap.Container container)
        {
            this.container = container;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IKeyBasedUseCase<TService>)useCase);
        }

        public void Bind(IKeyBasedUseCase<TService> useCase)
        {
            Registry registry = new Registry();
            registry.ForRequestedType<TService>().CacheBy(InstanceScope.PerRequest).AddInstances(ex => ex.OfConcreteType(useCase.GetBoundType()).WithName(useCase.Key));
            container.Configure(configure => configure.AddRegistry(registry));
        }
    }
}