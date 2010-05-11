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
using Siege.ServiceLocation.Exceptions;
using StructureMap;
using StructureMap.Attributes;
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

        public StructureMapAdapter(string configFileName) : this(new StructureMap.Container(x => x.AddConfigurationFromXmlFile(configFileName))) { }

        public StructureMapAdapter(Container container)
        {
            this.container = container;
            Registry registry = new Registry();

            registry.ForRequestedType<Container>().TheDefault.IsThis(container);

            container.Configure(x => x.AddRegistry(registry));
        }

        public void RegisterBinding(Type baseBinding, Type targetBinding)
        {
            Registry registry = new Registry();
            registry.ForRequestedType(baseBinding).CacheBy(InstanceScope.PerRequest).TheDefaultIsConcreteType(targetBinding);
            container.Configure(x => x.AddRegistry(registry));
        }

        public void Dispose()
        {
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            Collection<object> objects = new Collection<object>();

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

        public object GetInstance(Type type)
        {
            return container.GetInstance(type);
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

        public object GetInstance(Type serviceType, string key)
        {
            object instance;

            try
            {
                instance = container.GetInstance(serviceType, key);
            }
            catch (StructureMapException ex)
            {
                throw new RegistrationNotFoundException(serviceType, key, ex);
            }

            if (instance == null) throw new RegistrationNotFoundException(serviceType, key);

            return instance;
        }

        public Type ConditionalUseCaseBinding
        {
            get { return typeof(ConditionalUseCaseBinding<>); }
        }

        public Type DefaultUseCaseBinding
        {
            get { return typeof(DefaultUseCaseBinding<>); }
        }

        public Type KeyBasedUseCaseBinding
        {
            get { return typeof(KeyBasedUseCaseBinding<>); }
        }

        public Type OpenGenericUseCaseBinding
        {
            get { return typeof(OpenGenericUseCaseBinding); }
        }
    }
}