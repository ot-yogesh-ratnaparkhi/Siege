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
using System.Reflection;
using Siege.ServiceLocation.Planning;

namespace Siege.ServiceLocation.SiegeAdapter.Maps
{
	public class TypeMap
	{
		private Hashtable entries = new Hashtable();

		public List<Type> GetRegisteredTypesMatching(Type type)
        {
            var keyList = entries.Keys;
            int keyCount = keyList.Count;
            var keys = new ArrayList(keyList);
            var types = new List<Type>();

            for(int i = 0; i < keyCount; i++)
            {
                var key = keys[i];
                var item = (TypeMapList)entries[key];
                var itemType = item.Type;

                if(itemType == type || itemType.IsAssignableFrom(type)) types.Add(type);
            }

		    return types;
		}

		public void Add(Type from, Type to, string key)
		{
			if(!entries.ContainsKey(from))
			{
				var list = new TypeMapList(from);
				list.Add(to, key);
				entries.Add(from, list);
			}
		}

		public bool Contains(Type type)
		{
			if(!entries.ContainsKey(type) && type.IsClass)
			{
				Add(type, type, null);
			}

		    return entries.ContainsKey(type);
		}

		private Type CreateGeneric(Type type, string name)
		{
			var definition = type.GetGenericTypeDefinition();
		    var entry = (TypeMapList)entries[definition];
		    var types = entry.MappedTypes;
			var item = GetMappedType(name, types) ?? entry.MappedTypes[0];

		    if (!item.To.IsGenericType) return item.To;

			return item.To.MakeGenericType(type.GetGenericArguments());
		}

		private bool CanConstructGenericType(Type type)
		{
			if (!type.IsGenericType) return false;

			if(entries.ContainsKey(type.GetGenericTypeDefinition())) return true;

			return false;
		}

		public MappedType GetMappedType(Type type, string name)
		{
			return SelectItem(type, name);
		}

		private MappedType SelectItem(Type type, string name)
		{
			if (!entries.ContainsKey(type) && CanConstructGenericType(type))
			{
				var generic = CreateGeneric(type, name);

				Add(generic, generic, null);

				type = generic;
			}

			if (!entries.ContainsKey(type)) return null;

		    var entry = (TypeMapList)entries[type];
		    var types = entry.MappedTypes;

		    return GetMappedType(name, types);
		}

	    private MappedType GetMappedType(string name, List<MappedType> types)
	    {
	        int typeCount = types.Count;

            for(int i = 0; i < typeCount; i++)
            {
                var type = types[i];

                if(type.Name == name) return type;
            }

	        return null;
	    }
	}

	internal class TypeMapList
	{
		public Type Type { get; private set; }
		public List<MappedType> MappedTypes { get; private set; }
		private List<Type> registeredTypes = new List<Type>();

		public TypeMapList(Type type)
		{
			this.Type = type;
			this.MappedTypes = new List<MappedType>();
		}

		public void Add(Type to, string name)
		{
			if (this.registeredTypes.Contains(to)) return;
			if (!to.IsClass) throw new ApplicationException("Cannot map type " + this.Type + " to the interface " + to + ". You must map to a class.");

			this.registeredTypes.Add(to);
			this.MappedTypes.Add(new MappedType(this.Type, to, name));
		}
	}

	public class MappedType
	{
		public string Name { get; private set; }
		public Type From { get; private set; }
		public Type To { get; private set; }
		public List<ConstructorCandidate> Candidates { get; private set; }

		public MappedType(Type from, Type to, string name)
		{
			this.Name = name;
			this.From = from;
			this.To = to;
			this.Candidates = new List<ConstructorCandidate>();

			BuildCandidateList();
		}

		private void BuildCandidateList()
		{
			var constructors = To.GetConstructors();
		    var constructorCount = constructors.Length;
            for (int counter = 0; counter < constructorCount; counter++)
            {
                var constructor = constructors[counter];
                var candidate = new ConstructorCandidate { Type = To };
                var parameters = constructor.GetParameters();
                var count = parameters.Length;

                for (int i = 0; i < count; i++)
                {
                    ParameterInfo parameter = parameters[i];
                    var summary = new ParameterSummary
                    {
                        Position = parameter.Position,
                        ParameterType = parameter.ParameterType,
                        Name = parameter.Name
                    };

                    candidate.Parameters.Add(summary);
                }

                candidate.Instantiate = array => constructor.Invoke(array);

                Candidates.Add(candidate);
            }
		}
	}
}