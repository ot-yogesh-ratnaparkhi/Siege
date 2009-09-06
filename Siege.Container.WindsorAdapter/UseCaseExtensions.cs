using System;
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
            kernel.Register(Component.For(useCase.GetBoundType()).ToMethod(kernel, () => factory.Build()).Unless(Component.ServiceAlreadyRegistered));

        }

        public static void Bind<TBaseType>(this GenericUseCase<TBaseType> useCase, IKernel kernel)
        {
            kernel.Register(Component.For(useCase.GetBoundType()).ImplementedBy(useCase.GetBinding()).Unless(Component.ServiceAlreadyRegistered).LifeStyle.Transient);
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

    public static class ComponentRegistrationExtensions 
    {  
        public static ComponentRegistration<T> ToMethod<T, TS>(this ComponentRegistration<T> reg, IKernel kernel, Func<TS> factory) where TS: T 
        {  
            var factoryName = typeof(GenericFactory<TS>).FullName + Guid.NewGuid();  
            kernel.Register(Component.For<GenericFactory<TS>>().Named(factoryName).Instance(new GenericFactory<TS>(factory)));  
            reg.Configuration(Attrib.ForName("factoryId").Eq(factoryName), Attrib.ForName("factoryCreate").Eq("Create"));  
            
            return reg;  
        }  

        private class GenericFactory<T> 
        {  
            private readonly Func<T> factoryMethod;  

            public GenericFactory(Func<T> factoryMethod) 
            {  
                this.factoryMethod = factoryMethod;  
            }  

            public T Create() 
            {  
                return factoryMethod();  
            }  
        }  
    }  
}