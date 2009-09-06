using Siege.ServiceLocation;
using StructureMap;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Siege.Container.StructureMapAdapter
{
    public static class UseCaseExtensions
    {
        public static void Bind<TBaseType>(this IConditionalUseCase<TBaseType> useCase, IContextualServiceLocator locator)
        {
            Registry registry = new Registry();

            var factory = locator.GetInstance<ConditionalFactory<TBaseType>>();

            registry.ForRequestedType<TBaseType>().TheDefault.Is.ConstructedBy(factory.Build);
            ObjectFactory.Configure(configure => configure.AddRegistry(registry));
        }

        public static void Bind<TBaseType>(this GenericUseCase<TBaseType> useCase)
        {
            Registry registry = new Registry();
            registry.ForRequestedType<TBaseType>().TheDefault.Is.OfConcreteType(useCase.GetBinding());
            ObjectFactory.Configure(configure => configure.AddRegistry(registry));
        }

        public static void Bind<TBaseType>(this ImplementationUseCase<TBaseType> useCase)
        {
            Registry registry = new Registry();
            registry.ForRequestedType<TBaseType>().CacheBy(InstanceScope.Singleton).TheDefault.IsThis(useCase.GetBinding());
            ObjectFactory.Configure(configure => configure.AddRegistry(registry));
        }

        public static void Bind<TBaseType>(this KeyBasedUseCase<TBaseType> useCase)
        {
            Registry registry = new Registry();
            registry
                .ForRequestedType<TBaseType>()
                .AddInstances(x => x.OfConcreteType(useCase.GetBinding())
                                       .WithName(useCase.Key));
            ObjectFactory.Configure(configure => configure.AddRegistry(registry));
        }
    }
}