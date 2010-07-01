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
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Stores.UseCases
{
    public class ConditionalUseCaseList
    {
        private Dictionary<Type, List<IUseCase>> resolutionCases { get; set; }

        public ConditionalUseCaseList()
        {
            this.resolutionCases = new Dictionary<Type, List<IUseCase>>();
        }

        public List<IUseCase> GetUseCasesForType(Type type)
        {
            if (!Contains(type)) return null;

            return this.resolutionCases[type];
        }

        public void Add(Type type, IUseCase useCase)
        {
            if (!resolutionCases.ContainsKey(type))
            {
                var list = new List<IUseCase>();
                resolutionCases.Add(type, list);
            }

            List<IUseCase> selectedCase = GetUseCasesForType(type);

            selectedCase.Add(useCase);

            this.resolutionCases[type] = selectedCase;
        }

        public bool Contains(Type type)
        {
            return this.resolutionCases.ContainsKey(type);
        }
    }
}