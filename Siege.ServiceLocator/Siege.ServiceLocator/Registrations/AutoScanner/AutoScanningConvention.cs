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
using Siege.ServiceLocator.Registrations.Conventions;

namespace Siege.ServiceLocator.Registrations.AutoScanner
{
    public class AutoScanningConvention : IConvention
	{
		private readonly List<Assembly> assemblies = new List<Assembly>();
		private readonly List<string> namespaces = new List<string>();
        private readonly List<Type> baseTypes = new List<Type>();

        protected void FromAssemblyContaining<TType>()
        {
            this.assemblies.Add(typeof (TType).Assembly);
        }

		protected void FromNamespaceOf<TType>()
		{
			this.namespaces.Add(typeof(TType).Namespace);
		}

        protected void ForTypesImplementing<TType>()
        {
            baseTypes.Add(typeof(TType));
        }
		
		protected virtual List<IRegistration> CreateRegistrations()
		{
            var registrations = new List<IRegistration>();

            foreach (var type in this.assemblies.Select(a => a.GetExportedTypes()).SelectMany(types => types))
            {
				if (type.IsGenericTypeDefinition || type.IsInterface) continue;
            	if (this.baseTypes.Count > 0)
            	{
            		registrations.AddRange((from baseType in this.baseTypes
            		                        where baseType.IsAssignableFrom(type)
											&& !baseType.IsGenericTypeDefinition
											&& (namespaces.Count == 0 || namespaces.Contains(type.Namespace))
            		                        select new AutoScannedRegistration(baseType, type)).Cast<IRegistration>());
            	}
            	else
            	{
					if (namespaces.Count == 0 || namespaces.Contains(type.Namespace))
					{
						foreach (var @interface in type.GetInterfaces())
						{
							if (@interface.IsGenericTypeDefinition) continue;
            				registrations.Add(new AutoScannedRegistration(@interface, type));
						}

						if (type.BaseType != typeof(object) 
							&& !type.BaseType.IsGenericTypeDefinition 
							&& !type.BaseType.IsAbstract)
						{
							registrations.Add(new AutoScannedRegistration(type.BaseType, type));
						}
					}
            	}
            }

			return registrations;
		}

        public Action<IServiceLocator> Build()
        {
            return serviceLocator => serviceLocator.Register(CreateRegistrations());
        }
    }
}