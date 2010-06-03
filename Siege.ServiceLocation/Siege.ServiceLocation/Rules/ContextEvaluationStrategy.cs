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
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.Stores;

namespace Siege.ServiceLocation.Rules
{
    public class ContextEvaluationStrategy : IRuleEvaluationStrategy
    {
        public bool IsValid(IActivationRule rule, IServiceLocatorStore context)
        {
            var items = MergeContextItems(context);
            for (int i = 0; i < items.Count; i++)
            {
                var contextItem = items[i];
                if (rule.Evaluate(contextItem)) return true;
            }

            return false;
        }

        private static List<object> MergeContextItems(IServiceLocatorStore context)
        {
            var contextItems = new List<object>();
            var resolutionItems = context.ResolutionStore.Items;
            for(int i = 0; i < resolutionItems.Count; i++)
            {
                var argument = resolutionItems[i];
                if (argument is ContextArgument) contextItems.Add(((ContextArgument)argument).ContextItem);
            }

            var items = context.ContextStore.Items;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                contextItems.Add(item);
            }
            return contextItems;
        }
    }
}