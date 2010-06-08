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
using Ninject;
using Ninject.Parameters;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.ExtensionMethods;
using Siege.ServiceLocation.Resolution;

namespace Siege.ServiceLocation.NinjectAdapter
{
    public class NinjectAdapter : IServiceLocatorAdapter
    {
        private IKernel kernel;

        public NinjectAdapter()
            : this(new StandardKernel())
        {
        }

        public NinjectAdapter(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void Dispose()
        {
            kernel.Dispose();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return kernel.GetAll<TService>();
        }

		public object GetInstance(Type type, params IResolutionArgument[] parameters)
		{
			var args = new List<ConstructorArgument>();

            var constructorParameters = parameters.OfType<ConstructorParameter, IResolutionArgument>();
            for (int i = 0; i < constructorParameters.Length; i++)
            {
                var parameter = constructorParameters[i];
				args.Add(new ConstructorArgument(parameter.Name, parameter.Value));
			}

            return kernel.Get(type, args.OfType<ConstructorArgument, ConstructorArgument>());
		}

		public bool HasTypeRegistered(Type type)
		{
			return kernel.TryGet(type) != null;
		}

		public object GetInstance(Type serviceType, string key, params IResolutionArgument[] parameters)
		{
			var args = new List<ConstructorArgument>();

            var constructorParameters = parameters.OfType<ConstructorParameter, IResolutionArgument>();
            for (int i = 0; i < constructorParameters.Length; i++)
            {
                var parameter = constructorParameters[i];
				args.Add(new ConstructorArgument(parameter.Name, parameter.Value));
			}

            var instance = kernel.Get(serviceType, key, args.OfType<ConstructorArgument, ConstructorArgument>());

			if (instance == null) throw new RegistrationNotFoundException(serviceType, key);

			return instance;
		}

        public void Register(Type from, Type to)
        {
            kernel.Bind(from).To(to);
        }

        public void RegisterInstance(Type type, object instance)
        {
            kernel.Bind(type).ToConstant(instance);
        }

        public void RegisterWithName(Type from, Type to, string name)
        {
            kernel.Bind(from).To(to).Named(name);
        }

        public void RegisterInstanceWithName(Type type, object instance, string name)
        {
            kernel.Bind(type).ToConstant(instance).Named(name);
        }

        public void RegisterFactoryMethod(Type type, Func<object> func)
        {
            kernel.Bind(type).ToMethod(context => func());
        }
    }
}