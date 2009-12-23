using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Siege.ServiceLocation.StructureMapAdapter
{
    public class DefaultInstanceUseCaseBinding<TService> : IDefaultInstanceUseCaseBinding<TService>
    {
        private readonly StructureMap.Container container;
        private IServiceLocatorAdapter locator;

        public DefaultInstanceUseCaseBinding(StructureMap.Container container, IServiceLocatorAdapter locator)
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
            Registry registry = new Registry();

            var factory = (Factory<TService>)locator.GetFactory<TService>();

            factory.AddCase(useCase);

            if (typeof(TService) != useCase.GetBoundType())
            {
                registry.ForRequestedType<TService>().CacheBy(InstanceScope.PerRequest).TheDefault.Is.ConstructedBy(context => factory.Build());
            }

            var registrar = (typeof(DefaultInstanceUseCaseBinding<TService>)).GetMethod("BindInstance").MakeGenericMethod(typeof(TService), useCase.GetBinding().GetType());
            registrar.Invoke(useCase, new object[] { useCase, registry });

            container.Configure(configure => configure.AddRegistry(registry));
        }

        public static void BindInstance<TBaseType, TInstanceType>(DefaultInstanceUseCase<TBaseType> useCase, Registry registry)
        {
            object instance = useCase.GetBinding();

            registry.ForRequestedType<TInstanceType>().CacheBy(InstanceScope.PerRequest).TheDefault.IsThis((TInstanceType)instance);
        }
    }
}