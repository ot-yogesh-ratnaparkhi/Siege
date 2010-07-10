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

namespace Siege.ServiceLocation.Registrations.Default
{
    public class DefaultResolutionStrategy : IResolutionStrategy, ITypeRequester
    {
        private readonly IInstanceResolver locator;
        private readonly IServiceLocatorStore accessor;
        public event TypeRequestedEventHandler TypeRequested;

        public DefaultResolutionStrategy(IInstanceResolver locator, IServiceLocatorStore accessor)
        {
            this.locator = locator;
            this.accessor = accessor;
            this.accessor.ExecutionStore.WireEvent(this);
        }

        public object Resolve(Type boundType, IActivationRule rule, IActivationStrategy activator)
        {
            RaiseTypeRequestedEvent(boundType);
            return activator.Resolve(locator, accessor);
        }

        private void RaiseTypeRequestedEvent(Type type)
        {
            if (this.TypeRequested != null) this.TypeRequested(type);
        }
    }
}