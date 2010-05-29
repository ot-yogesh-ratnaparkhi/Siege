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
	public class FactoryMap
	{
		private Dictionary<Type, FactoryMapList> entries = new Dictionary<Type,FactoryMapList>();

		public void Add(Type from, Func<object> to, string key)
		{
			if (!entries.ContainsKey(from))
			{
				var list = new FactoryMapList(from);
				list.Add(to, key);
				entries.Add(from, list);
			}
		}

		public List<Type> GetRegisteredTypesMatching(Type type)
		{
			return new List<Type>(entries.Keys.Where(k => k == type || k.IsAssignableFrom(type)));
		}

		public bool Contains(Type type)
		{
			return entries.ContainsKey(type);
		}

		public object Get(Type type, string key)
		{
			var factories = entries[type].MappedFactories;
			var factory = factories.Where(f => f.Name == key && !string.IsNullOrEmpty(key)).FirstOrDefault();
			
			if(factory == null)
			{
				factory = entries[type].MappedFactories.First();
			}
			return factory.To();
		}
	}

	internal class FactoryMapList
	{
		private List<Func<object>> registeredFactories = new List<Func<object>>();
		public List<MappedFactory> MappedFactories { get; private set; }
		public Type Type { get; private set; }

		public FactoryMapList(Type type)
		{
			this.Type = type;
			this.MappedFactories = new List<MappedFactory>();
		}

		public void Add(Func<object> to, string key)
		{
			if (this.registeredFactories.Contains(to)) return;

			this.registeredFactories.Add(to);
			this.MappedFactories.Add(new MappedFactory(this.Type, to, key));
		}
	}

	internal class MappedFactory
	{
		public string Name { get; private set; }
		public Type From { get; private set; }
		public Func<object> To { get; private set; }

		public MappedFactory(Type from, Func<object> to, string key)
		{
			this.From = from;
			this.To = to;
			this.Name = key;
		}
	}
}