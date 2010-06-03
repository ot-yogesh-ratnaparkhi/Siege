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
using System.Collections.ObjectModel;
using System.Linq;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.Resolution;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Siege.ServiceLocation.StructureMapAdapter
{
    public class StructureMapAdapter : IServiceLocatorAdapter
    {
        private Container container;

        public StructureMapAdapter()
            : this(new Container(x => x.IncludeConfigurationFromConfigFile = true))
        {
        }

        public StructureMapAdapter(string configFileName)
            : this(new Container(x => x.AddConfigurationFromXmlFile(configFileName)))
        {
        }

        public StructureMapAdapter(Container container)
        {
            this.container = container;
            var registry = new Registry();

            registry.For<Container>().Use(container);

            container.Configure(x => x.AddRegistry(registry));
        }

        public void Dispose()
        {
            //container.Dispose();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var objects = new Collection<object>();

            foreach (object item in container.GetAllInstances(serviceType))
            {
                objects.Add(item);
            }

            return objects;
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return container.GetAllInstances<TService>();
        }

        public bool HasTypeRegistered(Type type)
        {
            try
            {
                return container.GetInstance(type) != null;
            }
            catch
            {
                return false;
            }
        }

        public object GetInstance(Type type, string key, params IResolutionArgument[] parameters)
        {
            object instance;

			//lol thanks stucturemap for your inconsistent API, I /love/ using reflection to call methods.

            try
            {
                ExplicitArgsExpression expression = null;

                if (parameters.OfType<ConstructorParameter>().Count() > 0)
                {
                	var parameter1 = parameters.OfType<ConstructorParameter>().First();

					expression = container.With(parameter1.Name).EqualTo(parameter1.Value);

					if (parameters.Count() > 1)
					{
						var constructorArgs = parameters.OfType<ConstructorParameter>();

						for (int i = 1; i < constructorArgs.Count(); i++)
						{
							expression.With(constructorArgs.ElementAt(i).Name).EqualTo(constructorArgs.ElementAt(i).Value);
						}
					}
                }
            	var method = typeof (ExplicitArgsExpression).GetMethods().Where(m => m.IsGenericMethod && m.Name =="GetInstance" && m.GetParameters().Count() == 1).First();
				method = method.MakeGenericMethod(type);
				
				instance = expression != null ? method.Invoke(expression, new object[] { key }) : 
					container.GetInstance(type, key);
            }
            catch (StructureMapException ex)
            {
                throw new RegistrationNotFoundException(type, key, ex);
            }

            if (instance == null) throw new RegistrationNotFoundException(type, key);

            return instance;
        }

        public object GetInstance(Type type, params IResolutionArgument[] parameters)
        {
            ExplicitArgsExpression expression = null;

            if (parameters.OfType<ConstructorParameter>().Count() > 0)
            {
                ConstructorParameter parameter1 = parameters.OfType<ConstructorParameter>().First();

                expression = container.With(parameter1.Name).EqualTo(parameter1.Value);

                if (parameters.Count() > 1)
                {
                    IEnumerable<ConstructorParameter> constructorArgs = parameters.OfType<ConstructorParameter>();

                    for (int i = 1; i < constructorArgs.Count(); i++)
                    {
                        expression.With(constructorArgs.ElementAt(i).Name).EqualTo(constructorArgs.ElementAt(i).Value);
                    }
                }
            }

            return expression != null ? expression.GetInstance(type) : container.GetInstance(type);
        }

        public void Register(Type from, Type to)
        {
            var registry = new Registry();
            registry.For(from).LifecycleIs(InstanceScope.PerRequest).Use(to);
            container.Configure(configure => configure.AddRegistry(registry));
        }

        public void RegisterInstance(Type type, object instance)
        {
            var registry = new Registry();
            registry.For(type).Use(instance);
            container.Configure(configure => configure.AddRegistry(registry));
        }

        public void RegisterWithName(Type from, Type to, string name)
        {
            var registry = new Registry();
            registry.For(from).LifecycleIs(InstanceScope.PerRequest).Use(to).Named(name);
            container.Configure(configure => configure.AddRegistry(registry));
        }

        public void RegisterInstanceWithName(Type type, object instance, string name)
        {
            var registry = new Registry();
            registry.For(type).Use(instance).Named(name);
            container.Configure(configure => configure.AddRegistry(registry));
        }

        public void RegisterFactoryMethod(Type type, Func<object> func)
        {
            var registry = new Registry();
            registry.For(type).LifecycleIs(InstanceScope.PerRequest).Use(x => func());
            container.Configure(configure => configure.AddRegistry(registry));
        }
    }
}