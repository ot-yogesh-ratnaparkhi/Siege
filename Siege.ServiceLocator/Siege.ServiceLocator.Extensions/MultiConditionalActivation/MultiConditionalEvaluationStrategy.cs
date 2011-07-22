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

using System.Collections.Generic;
using Siege.ServiceLocator.ResolutionRules;

namespace Siege.ServiceLocator.Extensions.MultiConditionalActivation
{
    public class MultiConditionalEvaluationStrategy : ContextEvaluationStrategy
    {
        private List<IConditionalActivationRule> list;

        public MultiConditionalEvaluationStrategy(List<IConditionalActivationRule> list)
        {
            this.list = list;
        }

        public override bool IsValid(IActivationRule rule, IInstanceResolver resolver, InternalStorage.IServiceLocatorStore context)
        {
            var items = MergeContextItems(context);

            if (items.Count < list.Count) return false;
            
            for (int i = 0; i < items.Count; i++)
            {
                var contextItem = items[i];

                if (!EvaluateRules(resolver, contextItem)) return false;
            }

            return true;
        }

        private bool EvaluateRules(IInstanceResolver resolver, object contextItem)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].Evaluate(resolver, contextItem)) return true;
            }
            return false;
        }
    }
}