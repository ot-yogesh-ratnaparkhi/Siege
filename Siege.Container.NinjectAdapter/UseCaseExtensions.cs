using System;
using Ninject;
using Ninject.Planning.Bindings;
using Siege.ServiceLocation;

namespace Siege.Container.NinjectAdapter
{
    public static class UseCaseExtensions
    {
        public static void Bind<TBaseType>(this IConditionalUseCase<TBaseType> useCase, IKernel kernel, IServiceLocator locator, BindingBuilder<TBaseType> builder)
        {
            var factory = locator.GetInstance<ConditionalFactory<TBaseType>>();
            factory.AddCase(useCase);

            builder.ToMethod(context => factory.Build());
            kernel.AddBinding(builder.Binding);
        }

        public static void Bind<TBaseType>(this GenericUseCase<TBaseType> useCase, IKernel kernel, BindingBuilder<TBaseType> builder)
        {
            Type type = useCase.GetBinding();

            builder.To(type).InTransientScope();
            kernel.AddBinding(builder.Binding);
        }
        
        public static void Bind<TBaseType>(this ImplementationUseCase<TBaseType> useCase, IKernel kernel, BindingBuilder<TBaseType> builder)
        {
            builder.ToConstant(useCase.GetBinding()).InTransientScope();
            kernel.AddBinding(builder.Binding);
        }
        
        public static void Bind<TBaseType>(this KeyBasedUseCase<TBaseType> useCase, IKernel kernel, BindingBuilder<TBaseType> builder)
        {
            builder.To(useCase.GetBinding()).InTransientScope();
            builder.Named(useCase.Key);
            kernel.AddBinding(builder.Binding);
        }
    }
}