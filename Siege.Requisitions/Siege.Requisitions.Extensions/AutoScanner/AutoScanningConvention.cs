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
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Registrations;

namespace Siege.Requisitions.Extensions.AutoScanner
{
    public class AutoScanningConvention : IConvention
    {
        private Assembly assembly;
        private readonly List<Type> baseTypes = new List<Type>();

        public AutoScanningConvention()
        {
            assembly = Assembly.GetCallingAssembly();
        }

        protected void FromAssemblyContaining<TType>()
        {
            assembly = typeof (TType).Assembly;
        }

        protected void ForTypesImplementing<TType>()
        {
            baseTypes.Add(typeof(TType));
        }

        public List<IRegistration> Build()
        {
            var registrations = new List<IRegistration>();

            foreach(Type type in assembly.GetExportedTypes())
            {
                if(baseTypes.Count > 0)
                {
                    registrations.AddRange((from baseType in baseTypes
                                       where baseType.IsAssignableFrom(type) && !type.IsInterface
                                       select new AutoScannedRegistration(baseType, type)).Cast<IRegistration>());
                }
                else
                {
                    registrations.Add(new AutoScannedRegistration(type.BaseType, type));
                }
            }

            return registrations;
        }
    }
}