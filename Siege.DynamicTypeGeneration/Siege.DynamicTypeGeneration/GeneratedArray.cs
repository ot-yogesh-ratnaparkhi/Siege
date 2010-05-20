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
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class GeneratedArray : ILocalIndexer
    {
        protected readonly int localIndex;
        private readonly IList<ITypeGenerationAction> actions;
        private readonly GeneratedMethod method;

        public int LocalIndex
        {
            get { return localIndex; }
        }

        public GeneratedArray(int localIndex, IList<ITypeGenerationAction> actions, GeneratedMethod method)
        {
            this.localIndex = localIndex;
            this.actions = actions;
            this.method = method;
        }

        public void AssignFrom(Func<ILocalIndexer> item)
        {
            item();
            actions.Add(new VariableAssignmentAction(() => method.MethodBuilder(), localIndex));
        }

        public void SetValueAtIndex(GeneratedVariable variable, int index)
        {
            actions.Add(new SetValueAtIndexAction(method, this, variable, index));
        }
    }
}