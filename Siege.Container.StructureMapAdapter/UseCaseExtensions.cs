using Siege.ServiceLocation;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Siege.Container.StructureMapAdapter
{
    public static class UseCaseExtensions
    {
        public static void Bind<TBaseType>(this IConditionalUseCase<TBaseType> useCase, StructureMap.Container container, StructureMapAdapter locator)
        {
            Registry registry = new Registry();

            var factory = (StructureMapFactory<TBaseType>)locator.GetFactory<TBaseType>();
            factory.AddCase(useCase);

            registry.ForRequestedType<TBaseType>().CacheBy(InstanceScope.PerRequest).TheDefault.Is.ConstructedBy(context => factory.Build(null));
            registry.ForRequestedType(useCase.GetBoundType()).CacheBy(InstanceScope.PerRequest).AddType(useCase.GetBoundType());
            container.Configure(configure => configure.AddRegistry(registry));
        }

        public static void Bind<TBaseType>(this IDefaultUseCase<TBaseType> useCase, StructureMap.Container container, StructureMapAdapter locator)
        {
            Registry registry = new Registry();

            var factory = (StructureMapFactory<TBaseType>)locator.GetFactory<TBaseType>();
            factory.AddCase(useCase);

            if (typeof(TBaseType) != useCase.GetBoundType()) registry.ForRequestedType<TBaseType>().CacheBy(InstanceScope.PerRequest).TheDefault.Is.ConstructedBy(context => factory.Build(null));
            registry.ForRequestedType(useCase.GetBoundType()).CacheBy(InstanceScope.PerRequest).AddType(useCase.GetBoundType());
            container.Configure(configure => configure.AddRegistry(registry));
        }

        public static void Bind<TBaseType>(this ImplementationUseCase<TBaseType> useCase, StructureMap.Container container)
        {
            Registry registry = new Registry();
            registry.ForRequestedType<TBaseType>().CacheBy(InstanceScope.PerRequest).AddInstances(x => x.IsThis(useCase.GetBinding()));
            container.Configure(configure => configure.AddRegistry(registry));
        }

        public static void Bind<TBaseType>(this IKeyBasedUseCase<TBaseType> useCase, StructureMap.Container container)
        {
            Registry registry = new Registry();
            registry
                .ForRequestedType<TBaseType>().CacheBy(InstanceScope.PerRequest).AddInstances(ex => ex.OfConcreteType(useCase.GetBoundType()).WithName(useCase.Key));
            container.Configure(configure => configure.AddRegistry(registry));
        }
    }
}