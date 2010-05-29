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
using Siege.ServiceLocation.Planning;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.SiegeAdapter.Maps;

namespace Siege.ServiceLocation.SiegeAdapter.ConstructionStrategies
{
	public class ReflectionConstructionStrategy : IConstructionStrategy
	{
		public object Get(Type type, string key, ConstructorParameter[] parameters, ResolutionMap map)
		{
			if(map.FactoryMap.Contains(type)) return map.FactoryMap.Get(type, key);
			if(map.InstanceMap.Contains(type)) return map.InstanceMap.Get(type, key);
			
			if(!map.TypeMap.Contains(type) && type.IsClass)
			{
				map.TypeMap.Add(type, type, null);
			}

			var mappedType = map.TypeMap.GetMappedType(type, key);
			var constructorArgs = new List<object>();

			if (mappedType == null) return null;

			if(mappedType.Candidates == null)
			{
				if(parameters == null || parameters.Count() == 0) return Activator.CreateInstance(mappedType.To);

				foreach (ConstructorParameter parameter in parameters)
				{
					constructorArgs.Add(parameter.Value);
				}

				return Activator.CreateInstance(mappedType.To, constructorArgs.ToArray());  
			}

			var candidate = SelectConstructor(mappedType, map, parameters);

			foreach(Type arg in candidate.Parameters)
			{
				var value = Get(arg, null, null, map);
				if(value != null) constructorArgs.Add(value);
			}

			foreach(ConstructorParameter parameter in parameters)
			{
				constructorArgs.Add(parameter.Value);
			}

			return Activator.CreateInstance(mappedType.To, constructorArgs.ToArray());  
		}

		public IEnumerable<object> GetAll(Type type, ResolutionMap map)
		{
			List<object> list = new List<object>();

			foreach(Type registration in map.GetAllRegisteredTypesMatching(type))
			{
				list.Add(Get(registration, null, new ConstructorParameter[] {}, map));
			}

			return list;
		}

		public IEnumerable<TService> GetAll<TService>(ResolutionMap map)
		{
			List<TService> list = new List<TService>();

			foreach (Type registration in map.GetAllRegisteredTypesMatching(typeof(TService)))
			{
				list.Add((TService)Get(registration, null, new ConstructorParameter[] { }, map));
			}

			return list;
		}

		private ConstructorCandidate SelectConstructor(MappedType type, ResolutionMap map, ConstructorParameter[] parameters)
		{
			foreach (ConstructorCandidate candidate in type.Candidates)
			{
				if (candidate.Parameters.All(p => map.Contains(p) || parameters.Any(param => p.IsAssignableFrom(param.Value.GetType()))))
				{
					return candidate;
				}
			}

			throw new Exception();
		}
	}
}