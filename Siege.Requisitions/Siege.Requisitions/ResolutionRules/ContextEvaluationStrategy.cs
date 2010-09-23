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
using System.Linq;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.Resolution;

namespace Siege.Requisitions.ResolutionRules
{
    public class ContextEvaluationStrategy : IRuleEvaluationStrategy
    {
        public virtual bool IsValid(IActivationRule rule, IServiceLocatorStore context)
        {
            var items = MergeContextItems(context);
            for (int i = 0; i < items.Count; i++)
            {
                var contextItem = items[i];
                if (rule.Evaluate(contextItem)) return true;
            }

            return false;
        }

        protected static List<object> MergeContextItems(IServiceLocatorStore context)
        {
            var contextItems = new List<object>(); 
            var stores = context.All<IResolutionStore>().ToList();
            var resolutionItems = new List<IResolutionArgument>();
            
            stores.ForEach(x => resolutionItems.AddRange(x.Items));
                
            for (int i = 0; i < resolutionItems.Count; i++)
            {
                var argument = resolutionItems[i];
                if (argument is ContextArgument) contextItems.Add(((ContextArgument) argument).ContextItem);
            }

            foreach (IContextStore contextStore in context.All<IContextStore>())
            {
                var items = contextStore.Items;

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    contextItems.Add(item);
                }
            }

            return contextItems;
        }
    }
}