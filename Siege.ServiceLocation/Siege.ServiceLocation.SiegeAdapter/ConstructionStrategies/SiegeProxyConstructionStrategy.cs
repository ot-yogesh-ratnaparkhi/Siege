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
using Siege.DynamicTypeGeneration;
using Siege.ServiceLocation.Planning;
using Siege.ServiceLocation.SiegeAdapter.Maps;

namespace Siege.ServiceLocation.SiegeAdapter.ConstructionStrategies
{
	public abstract class SiegeActivator
	{
		public abstract object Instantiate(object[] args);
	}

	public class SiegeProxyConstructionStrategy : IConstructionStrategy
	{
		private Dictionary<ConstructorCandidate, SiegeActivator> activators = new Dictionary<ConstructorCandidate,SiegeActivator>();
		private int registrationCount = 1;

		public bool CanConstruct(ConstructorCandidate candidate)
		{
			return activators.ContainsKey(candidate);
		}

		public object Create(ConstructorCandidate candidate, object[] parameters)
		{
			return this.activators[candidate].Instantiate(parameters);
		}

		public void Register(Type to, MappedType mappedType)
		{
			var generator = new TypeGenerator("Siege.DynamicTypes" + registrationCount);
			registrationCount++;

			if(mappedType == null) return;

			foreach (ConstructorCandidate candidate in mappedType.Candidates)
			{
				var activatorType = generator.CreateType(context =>
             	{
             		context.Named(Guid.NewGuid() + "Builder");
             		context.InheritFrom<SiegeActivator>();

					context.OverrideMethod<SiegeActivator>(activator => activator.Instantiate(null), method =>
					{
						method.WithBody(body =>
	                	{
							var instance = body.CreateVariable(to);
	                		
							List<ILocalIndexer> items = new List<ILocalIndexer>();

							foreach(ParameterInfo info in candidate.Parameters.OrderBy(p => p.Position))
							{
								var arg1 = body.CreateVariable(info.ParameterType);
								arg1.AssignFromParameter(new MethodParameter(info.Position+1));
								items.Add(arg1);
							}

	                		instance.AssignFrom(body.Instantiate(to, candidate.Parameters.OrderBy(p => p.Position).Select(p => p.ParameterType).ToArray(), items.ToArray()));
	                		body.Return(instance);
	                	});
					});
             	});

				activators.Add(candidate, (SiegeActivator)Activator.CreateInstance(activatorType));
			}

			generator.Save();
		}
	}
}