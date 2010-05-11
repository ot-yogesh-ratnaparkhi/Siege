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
using Siege.ServiceLocation.Exceptions;

namespace Siege.ServiceLocation.AutofacAdapter
{
    public class AutofacAdapter : IServiceLocatorAdapter
    {
        private IContainer container;

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

        public object GetInstance(Type type, string key)
        {
            try
            {
                return container.Resolve(key);
            }
            catch (ComponentNotRegisteredException)
            {
                throw new RegistrationNotFoundException(type, key);
            }
        }

        public object GetInstance(Type type)
        {
            return container.Resolve(type);
        }

        public bool HasTypeRegistered(Type type)
        {
            return container.IsRegistered(type);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
            object instance;
            if (container.TryResolve(type, out instance))
            {
                return ((IEnumerable)instance).Cast<object>();
            }

            return new List<object>();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            var type = typeof(IEnumerable<>).MakeGenericType(typeof(TService));
            object instance;
            if (container.TryResolve(type, out instance))
            {
                return ((IEnumerable)instance).Cast<TService>();
            }

            return new List<TService>();
        }

        public void RegisterBinding(Type baseBinding, Type targetBinding)
        {
            var builder = new ContainerBuilder();
            if(baseBinding.IsGenericType)
            {
                builder.RegisterGeneric(targetBinding).As(baseBinding);
            }
            else
            {
                builder.Register(targetBinding).As(baseBinding);
            }
            builder.Build(container);
        }

        public Type ConditionalUseCaseBinding
        {
            get { return typeof (ConditionalUseCaseBinding<>); }
        }

        public Type DefaultUseCaseBinding
        {
            get { return typeof (DefaultUseCaseBinding<>); }
        }

        public Type KeyBasedUseCaseBinding
        {
            get { return typeof (KeyBasedUseCaseBinding<>); }
        }

        public Type OpenGenericUseCaseBinding
        {
            get { return typeof (OpenGenericUseCaseBinding); }
        }
    }
}
