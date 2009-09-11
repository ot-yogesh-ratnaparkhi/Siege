using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.Container.WindsorAdapter
{
    public static class UseCaseExtensions
    {
        public static void Bind<TBaseType>(this IConditionalUseCase<TBaseType> useCase, IKernel kernel, WindsorAdapter locator)
        {
            var factory = (WindsorFactory<TBaseType>)locator.GetFactory<TBaseType>();
            factory.AddCase(useCase);

            kernel.Register(Component.For<TBaseType>().UsingFactoryMethod(() => factory.Build(null)).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
            kernel.Register(Component.For(useCase.GetBoundType()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }

        public static void Bind<TBaseType>(this IDefaultUseCase<TBaseType> useCase, IKernel kernel, WindsorAdapter locator)
        {
            var factory = (WindsorFactory<TBaseType>)locator.GetFactory<TBaseType>();
            factory.AddCase(useCase);

            if(typeof(TBaseType) != useCase.GetBoundType()) kernel.Register(Component.For<TBaseType>().UsingFactoryMethod(() => factory.Build(null)).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
            kernel.Register(Component.For(useCase.GetBoundType()).ImplementedBy(useCase.GetBoundType()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }

        public static void Bind<TBaseType>(this KeyBasedImplementationUseCase<TBaseType> useCase, IKernel kernel)
        {
            kernel.Register(Component.For(useCase.GetBoundType()).Instance(useCase.GetBinding()).Named(useCase.Key));
        }

        public static void Bind<TBaseType>(this ImplementationUseCase<TBaseType> useCase, IKernel kernel)
        {
            kernel.Register(Component.For(useCase.GetBoundType()).Instance(useCase.GetBinding()).Unless(Component.ServiceAlreadyRegistered));
        }

        public static void Bind<TBaseType>(this KeyBasedUseCase<TBaseType> useCase, IKernel kernel)
        {
            kernel.Register(Component.For(useCase.GetBoundType()).ImplementedBy(useCase.GetBinding()).Named(useCase.Key).Unless(Component.ServiceAlreadyRegistered).LifeStyle.Transient);
        }
    }
}