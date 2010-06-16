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
using Rhino.Mocks;
using Siege.ServiceLocation.Planning;
using Siege.ServiceLocation.SiegeAdapter.ConstructionStrategies;
using Siege.ServiceLocation.SiegeAdapter.Maps;

namespace Siege.ServiceLocation.RhinoMocksAdapter
{
	public class RhinoMocksConstructionStrategy : IConstructionStrategy
	{
		private readonly MockRepository repository = new MockRepository();
		private readonly Dictionary<Type, object> registeredInstances = new Dictionary<Type, object>();

		public object Create(ConstructorCandidate candidate, object[] parameters)
		{
			if (!registeredInstances.ContainsKey(candidate.Type))
			{
			    registeredInstances.Add(candidate.Type, repository.Stub(candidate.Type, parameters));
			}

			return registeredInstances[candidate.Type];
		}

		public void Register(Type to, MappedType mappedType)
		{
			if (!registeredInstances.ContainsKey(to) && to.IsInterface)
			{
				registeredInstances.Add(to, repository.DynamicMock(to));
			}
		}

	    public bool CanConstruct(ConstructorCandidate candidate)
	    {
	        return true;
	    }
	}
}