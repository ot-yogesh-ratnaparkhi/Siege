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
using System.Linq.Expressions;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.Named;
using Siege.ServiceLocator.Resolution.Pipeline;

namespace Siege.ServiceLocator.RegistrationTemplates.Named
{
    public class NamedRegistrationTemplate : AbstractRegistrationTemplate
    {
        public override void Register(IServiceLocatorAdapter adapter, IServiceLocatorStore store, IRegistration registration, ResolutionPipeline pipeline)
        {
            var namedRegistration = (INamedRegistration) registration;

            var mappedToType = registration.GetMappedToType();
            var mappedFromType = registration.GetMappedFromType();

            adapter.RegisterWithName(mappedToType, mappedToType, namedRegistration.Key);
            adapter.RegisterWithName(mappedFromType, mappedToType, namedRegistration.Key);

            RegisterNamedLazy(adapter, mappedFromType, namedRegistration.Key);
            RegisterNamedLazy(adapter, mappedToType, namedRegistration.Key);
        }

        protected void RegisterNamedLazy(IServiceLocatorAdapter adapter, Type type, string key)
        {
            var serviceLocator = (IServiceLocator)adapter.GetInstance(typeof (IServiceLocator));
            Type lazyLoader = typeof(Func<,>).MakeGenericType(typeof(string), type);

            Expression<Func<string, object>> func = x => serviceLocator.GetInstance(type, x);
            var parameter = Expression.Parameter(typeof (string), "param1");
            var lambda = Expression.Lambda(lazyLoader, Expression.Convert(Expression.Invoke(func, parameter), type), parameter).Compile();

            adapter.RegisterFactoryMethod(lazyLoader, () => lambda);
        }
    }
}