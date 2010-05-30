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

namespace Siege.ServiceLocation.SiegeAdapter.Maps
{
	public class InstanceMap
	{
		private Dictionary<Type, InstanceMapList> entries = new Dictionary<Type,InstanceMapList>();
		
		public List<Type> GetRegisteredTypesMatching(Type type)
		{
			return new List<Type>(entries.Keys.Where(k => k == type || k.IsAssignableFrom(type)));
		}
		
		public void Add(Type from, object to, string key)
		{
			if(!entries.ContainsKey(from))
			{
				var list = new InstanceMapList(from);
				list.Add(to, key);
				entries.Add(from, list);
			}
		}

		public bool Contains(Type type)
		{
			return entries.ContainsKey(type);
		}

		public object Get(Type type, string key)
		{
			var instances = entries[type].MappedInstances;
			var instance = instances.Where(f => f.Name == key && !string.IsNullOrEmpty(key)).FirstOrDefault() ??
			               entries[type].MappedInstances.First();

			return instance.To;
		}
	}

	internal class InstanceMapList
	{
		private List<object> registeredInstances = new List<object>();
		public List<MappedInstance> MappedInstances { get; private set; }
		public Type Type { get; private set; }

		public InstanceMapList(Type type)
		{
			this.Type = type;
			this.MappedInstances = new List<MappedInstance>();
		}

		public void Add(object to, string name)
		{
			if (this.registeredInstances.Contains(to)) return;

			this.registeredInstances.Add(to);
			this.MappedInstances.Add(new MappedInstance(this.Type, to, name));
		}
	}

	internal class MappedInstance
	{
		public string Name { get; private set; }
		public Type From { get; private set; }
		public object To { get; private set; }

		public MappedInstance(Type from, object to, string name)
		{
			this.Name = name;
			this.From = from;
			this.To = to;
		}
	}
}