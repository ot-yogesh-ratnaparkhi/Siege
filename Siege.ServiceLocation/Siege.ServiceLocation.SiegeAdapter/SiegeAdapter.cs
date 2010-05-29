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
using System.Linq;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.SiegeAdapter.ConstructionStrategies;

namespace Siege.ServiceLocation.SiegeAdapter
{
	public class SiegeAdapter : IServiceLocatorAdapter
	{
		private SiegeTypeResolver resolver;

		public SiegeAdapter() : this(new SiegeProxyConstructionStrategy())
		{
			
		}

		public SiegeAdapter(IConstructionStrategy strategy)
		{
			resolver = new SiegeTypeResolver(strategy);
		}

		public void Dispose()
		{
		}

		public object GetInstance(Type type, string key, params IResolutionArgument[] parameters)
		{
			var value = resolver.Get(type, key, parameters.OfType<ConstructorParameter>().ToArray());
			
			if(value == null) throw new RegistrationNotFoundException(type);

			return value;
		}

		public object GetInstance(Type type, params IResolutionArgument[] parameters)
		{
			var value = resolver.Get(type, parameters.OfType<ConstructorParameter>().ToArray());

			if (value == null) throw new RegistrationNotFoundException(type);

			return value;
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

		public void RegisterBinding(Type baseBinding, Type targetBinding)
		{
			resolver.Register(baseBinding, targetBinding);
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