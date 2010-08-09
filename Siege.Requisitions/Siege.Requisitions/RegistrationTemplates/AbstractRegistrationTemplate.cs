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
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Resolution;

namespace Siege.Requisitions.RegistrationTemplates
{
    public abstract class AbstractRegistrationTemplate : IRegistrationTemplate
    {
        public abstract void Register(IServiceLocatorAdapter adapter, IRegistration registration, IResolutionTemplate template);

        protected void RegisterLazy(IServiceLocatorAdapter adapter, Type type, IResolutionTemplate resolutionTemplate)
        {
            Type lazyLoader = typeof (Func<>).MakeGenericType(type);

            Expression<Func<object>> func = () => resolutionTemplate.Resolve(type);

            var lambda = Expression.Lambda(lazyLoader, Expression.Convert(Expression.Invoke(func), type)).Compile();
            
            adapter.RegisterFactoryMethod(lazyLoader, () => lambda);
        }
    }
}