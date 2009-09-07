using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.Container.WindsorAdapter
{
    public static class UseCaseExtensions
    {
        public static void Bind<TBaseType>(this IConditionalUseCase<TBaseType> useCase, IKernel kernel, IContextualServiceLocator locator)
        {
            var factory = locator.GetConditionalFactory<TBaseType>();
            factory.AddCase(useCase);

            kernel.Register(Component.For<TBaseType>().UsingFactoryMethod(() => factory.Build()).Unless(Component.ServiceAlreadyRegistered));
            kernel.Register(Component.For(useCase.GetBoundType()).Unless(Component.ServiceAlreadyRegistered));
        }

        public static void Bind<TBaseType>(this GenericUseCase<TBaseType> useCase, IKernel kernel)
        {
            kernel.Register(Component.For(useCase.GetBoundType(), typeof(TBaseType)).ImplementedBy(useCase.GetBinding()).LifeStyle.Transient);
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