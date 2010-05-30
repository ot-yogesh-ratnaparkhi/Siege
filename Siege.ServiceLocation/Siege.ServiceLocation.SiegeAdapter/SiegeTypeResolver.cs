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
using System.Reflection;
using Siege.ServiceLocation.Planning;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.SiegeAdapter.ConstructionStrategies;
using Siege.ServiceLocation.SiegeAdapter.Maps;

namespace Siege.ServiceLocation.SiegeAdapter
{
	internal class SiegeTypeResolver
	{
		private readonly IConstructionStrategy strategy;
		protected ResolutionMap resolutionMap = new ResolutionMap();

		public SiegeTypeResolver(IConstructionStrategy strategy)
		{
			this.strategy = strategy;
			Register(typeof(SiegeTypeResolver), this, null);
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

		public void Register(Type from, string key)
		{
			Register(from, from, key);
		}

		public virtual void Register(Type from, object instance, string key)
		{
			this.resolutionMap.InstanceMap.Add(from, instance, key);
		}

		public virtual void Register(Type from, Type to, string key)
		{
			this.resolutionMap.TypeMap.Add(from, to, key);
			strategy.Register(to, resolutionMap.TypeMap.GetMappedType(to, key));
		}

		public virtual void RegisterWithFactoryMethod(Type from, Func<object> to, string key)
		{
			this.resolutionMap.FactoryMap.Add(from, to, key);
		}

		public bool Contains(Type type)
		{
			return this.resolutionMap.Contains(type);
		}

		public void RegisterWithFactoryMethod(Type from, Func<object> to)
		{
			RegisterWithFactoryMethod(from, to, null);
		}

		public object Get(Type type, string key, ConstructorParameter[] parameters)
		{
			if (resolutionMap.FactoryMap.Contains(type)) return resolutionMap.FactoryMap.Get(type, key);
			if (resolutionMap.InstanceMap.Contains(type)) return resolutionMap.InstanceMap.Get(type, key);

			if (!resolutionMap.TypeMap.Contains(type) && type.IsClass)
			{
				resolutionMap.TypeMap.Add(type, type, null);
			}

			var mappedType = resolutionMap.TypeMap.GetMappedType(type, key);

			if (mappedType == null) return null;

			var candidate = SelectConstructor(mappedType, resolutionMap, parameters);

			var constructorArgs = new object[candidate.Parameters.Count];

			foreach (ParameterInfo arg in candidate.Parameters)
			{
				var value = Get(arg.ParameterType, null, null);

				if (value != null)
				{
					constructorArgs[arg.Position] = value;
				}
				else
				{
					foreach (ConstructorParameter parameter in parameters)
					{
						if (parameter.Name == arg.Name)
						{
							constructorArgs[arg.Position] = parameter.Value;
						}
					}
				}

			}

			if (!strategy.CanConstruct(candidate)) strategy.Register(candidate.Type, mappedType);

			return strategy.Create(candidate, constructorArgs.ToArray());  
		}

		public object Get(Type type, ConstructorParameter[] parameters)
		{
			return Get(type, null, parameters);
		}

		public bool IsRegistered(Type type)
		{
			return resolutionMap.Contains(type);
		}

		public IEnumerable<object> GetAll(Type type)
		{
			List<object> list = new List<object>();

			foreach (Type registration in resolutionMap.GetAllRegisteredTypesMatching(type))
			{
				list.Add(Get(registration, null, new ConstructorParameter[] { }));
			}

			return list;
		}

		public IEnumerable<TService> GetAll<TService>()
		{
			List<TService> list = new List<TService>();

			foreach (Type registration in resolutionMap.GetAllRegisteredTypesMatching(typeof(TService)))
			{
				list.Add((TService)Get(registration, null, new ConstructorParameter[] { }));
			}

			return list;
		}

		private ConstructorCandidate SelectConstructor(MappedType type, ResolutionMap map, ConstructorParameter[] parameters)
		{
			foreach (ConstructorCandidate candidate in type.Candidates)
			{
				if (candidate.Parameters.All(p => map.Contains(p.ParameterType) || parameters.Any(param => p.ParameterType.IsAssignableFrom(param.Value.GetType()))))
				{
					return candidate;
				}
			}

			throw new Exception();
		}
	}
}