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
using Microsoft.Practices.Unity;
using Siege.ServiceLocation.Exceptions;

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
            container.Dispose();
        }

        public object GetInstance(Type type, string key)
        {
            try
            {
                return container.Resolve(type, key);
            }
            catch (ResolutionFailedException)
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

        public void RegisterBinding(Type baseBinding, Type targetBinding)
        {
            container.RegisterType(baseBinding, targetBinding);
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
    }
}
