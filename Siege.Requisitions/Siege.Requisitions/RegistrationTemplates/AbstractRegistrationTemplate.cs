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
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Resolution;
using Siege.Requisitions.Resolution.Pipeline;

namespace Siege.Requisitions.RegistrationTemplates
{
    public abstract class AbstractRegistrationTemplate : IRegistrationTemplate
    {
        public abstract void Register(IServiceLocatorAdapter adapter, IServiceLocatorStore store, IRegistration registration, ResolutionPipeline pipeline);

        protected void RegisterLazy(IServiceLocatorAdapter adapter, Type type, ResolutionPipeline pipeline)
        {
            Type lazyLoader = typeof (Func<>).MakeGenericType(type);

            Expression<Func<object>> func = () => pipeline.Execute(type);

            var lambda = Expression.Lambda(lazyLoader, Expression.Convert(Expression.Invoke(func), type)).Compile();
            
            adapter.RegisterFactoryMethod(lazyLoader, () => lambda);
        }

		protected void RegisterContextual(IServiceLocatorAdapter adapter, Type type)
		{
			var serviceLocator = (IServiceLocator)adapter.GetInstance(typeof(IServiceLocator));
			Type lazyLoader = typeof(Func<,>).MakeGenericType(typeof(object), type);

			Expression<Func<object, object>> func = x => serviceLocator.GetInstance(type, new ContextArgument(x));
			var parameter = Expression.Parameter(typeof(object), "param1");
			var lambda = Expression.Lambda(lazyLoader, Expression.Convert(Expression.Invoke(func, parameter), type), parameter).Compile();

			adapter.RegisterFactoryMethod(lazyLoader, () => lambda);
		}

		protected void RegisterTypeResolver(IServiceLocatorAdapter adapter, Type type)
		{
			Type lazyLoader = typeof(Func<,>).MakeGenericType(typeof(Type), type);
			if (adapter.HasTypeRegistered(lazyLoader)) return;
			
			var serviceLocator = (IServiceLocator)adapter.GetInstance(typeof(IServiceLocator));

			Expression<Func<object, object>> func = x => serviceLocator.GetInstance((Type)x);
			var parameter = Expression.Parameter(typeof(object), "param1");
			var lambda = Expression.Lambda(lazyLoader, Expression.Convert(Expression.Invoke(func, parameter), type), parameter).Compile();

			adapter.RegisterFactoryMethod(lazyLoader, () => lambda);
		}
    }
}