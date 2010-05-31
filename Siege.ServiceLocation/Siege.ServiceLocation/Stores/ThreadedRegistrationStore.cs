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
using Siege.ServiceLocation.EventHandlers;
using Siege.ServiceLocation.Planning;

namespace Siege.ServiceLocation.Stores
{
	public class ThreadedRegistrationStore : IRegistrationStore
	{
		private Dictionary<Type, List<ConstructorCandidate>> registeredTypes = new Dictionary<Type, List<ConstructorCandidate>>();

		public List<Type> RegisteredTypes
		{
			get
			{
				return new List<Type>(registeredTypes.Keys.ToArray());
			}
		}

		public List<ConstructorCandidate> GetCandidatesForType<TType>() where TType : class
		{
			if(!registeredTypes.ContainsKey(typeof(TType))) return null;

			return registeredTypes[typeof (TType)];
		}

		public void WireEvent(ITypeRegistrar typeRegistrar)
		{
			//typeRegistrar.TypeRegistered += OnTypeRegistered;
		}

		void OnTypeRegistered(Type type)
		{
			if (registeredTypes.ContainsKey(type)) return;

			var candidates = new List<ConstructorCandidate>();
		    var constructors = type.GetConstructors();
		    var constructorCount = constructors.Length;
            for (int counter = 0; counter < constructorCount; counter++)
            {
                var constructor = constructors[counter];
                var candidate = new ConstructorCandidate { Type = type };
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

                candidates.Add(candidate);
            }

			registeredTypes.Add(type, candidates);
		}
	}
}