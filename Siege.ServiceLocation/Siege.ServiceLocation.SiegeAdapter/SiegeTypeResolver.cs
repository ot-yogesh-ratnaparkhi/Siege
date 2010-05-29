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
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.SiegeAdapter.ConstructionStrategies;
using Siege.ServiceLocation.SiegeAdapter.Maps;

namespace Siege.ServiceLocation.SiegeAdapter
{
	internal class SiegeTypeResolver
	{
		private readonly IConstructionStrategy strategy;

		private ResolutionMap resolutionMap = new ResolutionMap();

		public SiegeTypeResolver(IConstructionStrategy strategy)
		{
			this.strategy = strategy;
			this.resolutionMap.InstanceMap.Add(typeof(SiegeTypeResolver), this, null);
		}

		public void Register(Type from, Type to)
		{
			Register(from, to, null);
		}

		public void Register(Type type)
		{
			Register(type, type);
		}

		public void Register(Type from, object instance)
		{
			Register(from, instance, null);
		}

		public void Register(Type from, object instance, string key)
		{
			this.resolutionMap.InstanceMap.Add(from, instance, key);
		}

		public void Register(Type from, string key)
		{
			Register(from, from, key);
		}

		public void Register(Type from, Type to, string key)
		{
			this.resolutionMap.TypeMap.Add(from, to, key);
		}

		public void RegisterWithFactoryMethod(Type from, Func<object> to)
		{
			RegisterWithFactoryMethod(from, to, null);
		}

		public void RegisterWithFactoryMethod(Type from, Func<object> to, string key)
		{
			this.resolutionMap.FactoryMap.Add(from, to, key);
		}

		public object Get(Type type, string key, ConstructorParameter[] parameters)
		{
			return strategy.Get(type, key, parameters, resolutionMap);
		}

		public object Get(Type type, ConstructorParameter[] parameters)
		{
			return Get(type, null, parameters);
		}

		public bool IsRegistered(Type type)
		{
			return this.resolutionMap.Contains(type);
		}

		public IEnumerable<object> GetAll(Type type)
		{
			return strategy.GetAll(type, resolutionMap); ;
		}

		public IEnumerable<TService> GetAll<TService>()
		{
			return strategy.GetAll<TService>(resolutionMap); ;
		}
	}
}