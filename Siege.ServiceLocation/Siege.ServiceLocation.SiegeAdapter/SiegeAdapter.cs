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
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.SiegeAdapter.ConstructionStrategies;

namespace Siege.ServiceLocation.SiegeAdapter
{
    public class SiegeAdapter : IServiceLocatorAdapter
    {
        private readonly SiegeTypeResolver resolver;

        public SiegeAdapter() : this(new SiegeProxyConstructionStrategy())
        {
        }

        public SiegeAdapter(IConstructionStrategy strategy)
        {
            resolver = new SiegeTypeResolver(strategy);
            resolver.Register(typeof(SiegeTypeResolver), resolver, null);
        }

        public void Dispose()
        {
        }

        public object GetInstance(Type type, string key, params IResolutionArgument[] parameters)
        {
            object value = resolver.Get(type, key, GetConstructorParameters(parameters));

            if (value == null) throw new RegistrationNotFoundException(type);

            return value;
        }

        public object GetInstance(Type type, params IResolutionArgument[] parameters)
        {
            object value = resolver.Get(type, GetConstructorParameters(parameters));

            if (value == null) throw new RegistrationNotFoundException(type);

            return value;
        }

        public ConstructorParameter[] GetConstructorParameters(IResolutionArgument[] parameters)
        {
            int parameterCount = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterCount++;
            }

            var constructorParameters = new ConstructorParameter[parameterCount];
            int currentParameterIndex = 0;

            for (int i = 0; i < parameters.Length; i++)
            {
                IResolutionArgument argument = parameters[i];
                if(argument is ConstructorParameter)
                {
                    constructorParameters[currentParameterIndex] = (ConstructorParameter)argument;
                    currentParameterIndex++;
                }
            }

            return constructorParameters;
        }

        public bool HasTypeRegistered(Type type)
        {
            return resolver.IsRegistered(type);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return resolver.GetAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return resolver.GetAll<TService>();
        }

        public void Register(Type from, Type to)
        {
            resolver.Register(from, to);
            resolver.Register(to, to);
        }

        public void RegisterInstance(Type type, object instance)
        {
            resolver.Register(type, instance);
        }

        public void RegisterWithName(Type from, Type to, string name)
        {
            resolver.Register(from, to, name);
        }

        public void RegisterInstanceWithName(Type type, object instance, string name)
        {
            resolver.Register(type, instance, name);
        }

        public void RegisterFactoryMethod(Type type, Func<object> func)
        {
            resolver.RegisterWithFactoryMethod(type, func);
        }
    }
}