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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Siege.ServiceLocator.ExtensionMethods;
using Siege.ServiceLocator.Exceptions;
using Siege.ServiceLocator.Resolution;

namespace Siege.ServiceLocator.AutofacAdapter
{
    public class AutofacAdapter : IServiceLocatorAdapter
    {
        private readonly IContainer container;

        public AutofacAdapter(IContainer container)
        {
            this.container = container;
        }

        public AutofacAdapter() : this(new Container())
        {
            
        }
        public void Dispose()
        {
            container.Dispose();
        }

		public object GetInstance(Type type, string key, params IResolutionArgument[] parameters)
        {
            try
			{
				var args = new List<NamedParameter>();

                var constructorParameters = parameters.OfType<ConstructorParameter, IResolutionArgument>();
                for (int i = 0; i < constructorParameters.Length; i++)
                {
                    var parameter = constructorParameters[i];
					args.Add(new NamedParameter(parameter.Name, parameter.Value));
				}

                return container.Resolve(key, args.OfType<NamedParameter, NamedParameter>());
            }
            catch (ComponentNotRegisteredException)
            {
                throw new RegistrationNotFoundException(type, key);
            }
        }

		public object GetInstance(Type type, params IResolutionArgument[] parameters)
		{
			var args = new List<NamedParameter>();

            var constructorParameters = parameters.OfType<ConstructorParameter, IResolutionArgument>();
            for (int i = 0; i < constructorParameters.Length; i++)
            {
                var parameter = constructorParameters[i];
				args.Add(new NamedParameter(parameter.Name, parameter.Value));
			}

            return container.Resolve(type, args.OfType<NamedParameter, NamedParameter>());
        }

        public TService GetInstance<TService>(Type type, params IResolutionArgument[] arguments)
        {
            return (TService)GetInstance(type, arguments);
        }

        public TService GetInstance<TService>(string key, params IResolutionArgument[] arguments)
        {
            return (TService)GetInstance(typeof(TService), key, arguments);
        }

        public TService GetInstance<TService>(params IResolutionArgument[] arguments)
        {
            return (TService)GetInstance(typeof(TService), arguments);
        }

        public bool HasTypeRegistered(Type type)
        {
            return container.IsRegistered(type);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
            object instance;
            
            return container.TryResolve(type, out instance) ? ((IEnumerable)instance).Cast<object>() : new List<object>();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            var type = typeof(IEnumerable<>).MakeGenericType(typeof(TService));
            object instance;
            
            return container.TryResolve(type, out instance) ? ((IEnumerable)instance).Cast<TService>() : new List<TService>();
        }

        public void Register(Type from, Type to)
        {
            var builder = new ContainerBuilder();

            var genericType = typeof(IEnumerable<>).MakeGenericType(from);
            if (!from.IsGenericType && !container.IsRegistered(genericType))
            {
                builder.RegisterCollection(from).As(genericType);
            }

            if(!from.IsGenericType)
            {
                builder.Register(to).As(from).FactoryScoped();
            }
            else
            {
                builder.RegisterGeneric(to).As(from).FactoryScoped();
            }
            builder.Build(container);
        }

        public void RegisterInstance(Type type, object instance)
        {
            var builder = new ContainerBuilder();

            var genericType = typeof(IEnumerable<>).MakeGenericType(type);
            if (!container.IsRegistered(genericType))
            {
                builder.RegisterCollection(type).As(genericType);
            }

            builder.Register(instance).As(type);
            builder.Register(instance);

            builder.Build(container);
        }

        public void RegisterWithName(Type from, Type to, string name)
        {
            var builder = new ContainerBuilder();

            builder.Register(from).Named(name);
            builder.Register(to).Named(name);

            builder.Build(container);
        }

        public void RegisterInstanceWithName(Type type, object instance, string name)
        {
            var builder = new ContainerBuilder();

            builder.Register(c => type).Named(name);
            builder.Register(c => instance).Named(name);

            builder.Build(container);
        }

        public void RegisterFactoryMethod(Type type, Func<object> func)
        {
            var builder = new ContainerBuilder();
            var genericType = typeof(IEnumerable<>).MakeGenericType(type);
            
            if (!container.IsRegistered(genericType))
            {
                builder.RegisterCollection(type).As(genericType);
            }
            
            builder.Register((c => func())).As(type).FactoryScoped().MemberOf(genericType);
            
            builder.Build(container);
        }
    }
}
