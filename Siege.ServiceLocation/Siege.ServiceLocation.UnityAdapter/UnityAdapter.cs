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
using System.Linq;
using Microsoft.Practices.Unity;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.Resolution;

namespace Siege.ServiceLocation.UnityAdapter
{
    public class UnityAdapter : IServiceLocatorAdapter
    {
        private IUnityContainer container;

        public UnityAdapter(IUnityContainer container)
        {
            this.container = container;
        }

        public UnityAdapter() : this(new UnityContainer())
        {
            
        }

        public void Dispose()
        {
            //container.Dispose();
        }

		public object GetInstance(Type type, string key, params IResolutionArgument[] parameters)
        {
            try
			{
				ParameterOverrides args = new ParameterOverrides();

				foreach (ConstructorParameter parameter in parameters.OfType<ConstructorParameter>())
				{
					args.Add(parameter.Name, parameter.Value);
				}

                return container.Resolve(type, key, args);
            }
            catch (ResolutionFailedException)
            {
                throw new RegistrationNotFoundException(type, key);
            }
        }

		public object GetInstance(Type type, params IResolutionArgument[] parameters)
		{
			ParameterOverrides args = new ParameterOverrides();

			foreach (ConstructorParameter parameter in parameters.OfType<ConstructorParameter>())
			{
				args.Add(parameter.Name, parameter.Value);
			}

            return container.Resolve(type, args);
        }

        public bool HasTypeRegistered(Type type)
        {
            try
            {
                return container.Resolve(type) != null;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.ResolveAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return container.ResolveAll<TService>();
        }

        public void Register(Type from, Type to)
        {
            container.RegisterType(from, to);
        }

        public void RegisterInstance(Type type, object instance)
        {
            container.RegisterInstance(type, instance);
        }

        public void RegisterWithName(Type from, Type to, string name)
        {
            container.RegisterType(from, to, name);
        }

        public void RegisterInstanceWithName(Type type, object instance, string name)
        {
            container.RegisterInstance(type, name, instance);
        }

        public void RegisterFactoryMethod(Type type, Func<object> func)
        {
            container.RegisterType(type, new TransientLifetimeManager(), new InjectionFactory(f => func()));
            container.RegisterType(type, Guid.NewGuid().ToString(), new TransientLifetimeManager(), new InjectionFactory(f => func()));

        }
    }
}
