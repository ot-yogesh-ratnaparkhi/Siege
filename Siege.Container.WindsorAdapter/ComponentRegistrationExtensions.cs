using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;

namespace Siege.Container.WindsorAdapter
{
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