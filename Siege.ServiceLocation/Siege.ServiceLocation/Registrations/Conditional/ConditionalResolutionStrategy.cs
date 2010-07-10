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
using Siege.ServiceLocation.EventHandlers;
using Siege.ServiceLocation.InternalStorage;
using Siege.ServiceLocation.ResolutionRules;

namespace Siege.ServiceLocation.Registrations.Conditional
{
    public class ConditionalResolutionStrategy : IResolutionStrategy, ITypeRequester
    {
        private readonly IInstanceResolver locator;
        private readonly IServiceLocatorStore context;
        public event TypeRequestedEventHandler TypeRequested;

        public ConditionalResolutionStrategy(IInstanceResolver locator, IServiceLocatorStore context)
        {
            this.locator = locator;
            this.context = context;
            this.context.ExecutionStore.WireEvent(this);
        }

        public object Resolve(Type boundType, IActivationRule rule, IActivationStrategy activator)
        {
            if (rule == null) return null;

            if (rule.GetRuleEvaluationStrategy().IsValid(rule, context))
            {
                RaiseTypeRequestedEvent(boundType);
                return activator.Resolve(locator, context);
            }

            return null;
        }

        private void RaiseTypeRequestedEvent(Type type)
        {
            if (this.TypeRequested != null) this.TypeRequested(type);
        }
    }
}