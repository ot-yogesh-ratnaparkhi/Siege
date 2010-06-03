/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Collections.Generic;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.ExtensionMethods;
using Siege.ServiceLocation.Resolution;

namespace Siege.SeviceLocation.WindsorAdapter
{
    public class WindsorAdapter : IServiceLocatorAdapter
    {
        private IKernel kernel;

        public WindsorAdapter()
            : this(new DefaultKernel())
        {
        }

        public WindsorAdapter(IKernel kernel)
        {
            this.kernel = kernel;
            if(this.kernel.GetFacilities().OfType<FactorySupportFacility, IFacility>().Length == 0)
            {
            	this.kernel.AddFacility<FactorySupportFacility>();
            }
        }

        public void Dispose()
        {
            //bug in windsor lol
            //kernel.Dispose();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return (IEnumerable<object>)kernel.ResolveAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return kernel.ResolveAll<TService>();
        }

		public object GetInstance(Type type, string key, params IResolutionArgument[] parameters)
		{
			try
			{
				Dictionary<string, object> args = new Dictionary<string, object>();

                foreach (ConstructorParameter parameter in parameters.OfType<ConstructorParameter, IResolutionArgument>())
				{
					args.Add(parameter.Name, parameter.Value);
				}

				return kernel.Resolve(key, type, args);
			}
			catch (ComponentNotFoundException ex)
			{
				throw new RegistrationNotFoundException(type, key, ex);
			}
		}

		public object GetInstance(Type type, params IResolutionArgument[] parameters)
		{
			try
			{
				Dictionary<string, object> args = new Dictionary<string, object>();

                foreach (ConstructorParameter parameter in parameters.OfType<ConstructorParameter, IResolutionArgument>())
				{
					args.Add(parameter.Name, parameter.Value);
				}

				return kernel.Resolve(type, args);
			}
			catch (Exception ex)
			{
				throw new RegistrationNotFoundException(type, ex);
			}
		}

        public bool HasTypeRegistered(Type type)
        {
            return kernel.HasComponent(type);
        }

        public void Register(Type from, Type to)
        {
            if(kernel.HasComponent(from) || kernel.HasComponent(to)) return;
            kernel.Register(Component.For(from).ImplementedBy(to).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }

        public void RegisterInstance(Type type, object instance)
        {
            kernel.Register(Component.For(type).Instance(instance).Unless(Component.ServiceAlreadyRegistered));
        }

        public void RegisterWithName(Type from, Type to, string name)
        {
            if(kernel.HasComponent(name)) return;
            kernel.Register(Component.For(from).ImplementedBy(to).Named(name).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }

        public void RegisterInstanceWithName(Type type, object instance, string name)
        {
            if (kernel.HasComponent(name)) return;
            kernel.Register(Component.For(type).Instance(instance).Named(name).Unless(Component.ServiceAlreadyRegistered));
        }

        public void RegisterFactoryMethod(Type type, Func<object> func)
        {
            kernel.Register(Component.For(type).FactoryMethod(kernel, type, func).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }
    }

   public static class ComponentRegistrationExtensions 
   {
       public static ComponentRegistration FactoryMethod(this ComponentRegistration reg, IKernel kernel, Type type, Func<object> factory)
       {
           Type item = typeof(InternalFactory<>).MakeGenericType(type);
           var factoryName = item.FullName;
           kernel.Register(Component.For(item).Named(factoryName).Instance(new GenericFactory(factory)).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));  
           reg.Configuration(Attrib.ForName("factoryId").Eq(factoryName), Attrib.ForName("factoryCreate").Eq("Create"));  
           return reg;  
       }  
     
       private class InternalFactory<T> : GenericFactory
       {
           public InternalFactory(Func<object> factoryMethod) : base(factoryMethod)
           {
           }
       }
       private class GenericFactory 
       {  
           private readonly Func<object> factoryMethod;  
     
           public GenericFactory(Func<object> factoryMethod) 
           {  
               this.factoryMethod = factoryMethod;  
           }  
     
           public object Create() 
           {  
               return factoryMethod();  
           }  
       }  
   }  
}