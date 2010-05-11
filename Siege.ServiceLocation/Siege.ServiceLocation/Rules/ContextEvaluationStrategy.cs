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
            foreach (object contextItem in MergeContextItems(context))
            {
                if (rule.Evaluate(contextItem)) return true;
            }

            return false;
        }

        private List<object> MergeContextItems(IServiceLocatorStore context)
        {
            List<object> contextItems = new List<object>();

            foreach (IResolutionArgument argument in context.ResolutionStore.Items)
            {
                if (argument is ContextArgument) contextItems.Add(((ContextArgument)argument).ContextItem);
            }

            foreach (object item in context.ContextStore.Items)
            {
                contextItems.Add(item);
            }
            return contextItems;
        }
    }
}