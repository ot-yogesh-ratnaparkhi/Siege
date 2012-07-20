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
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.Stores;

namespace Siege.ServiceLocator.Resolution.Pipeline
{
    public class DefaultPostResolutionPipelineItem : BasePipelineItem<DefaultPostResolutionStore>
    {
        private readonly DefaultPostResolutionStore registrationStore;

        public DefaultPostResolutionPipelineItem(Foundation foundation, IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store) : base(foundation, serviceLocator, store)
        {
            registrationStore = foundation.StoreFor<DefaultPostResolutionStore>();
        }

        protected override Func<PipelineResult> Invoker(Type type, PipelineResult result)
        {
            return () =>
            {
                if (result == null) return null;

                IList<IRegistration> actions = null;

                if (registrationStore.Contains(result.MappedTo))
                {
                    actions = registrationStore.GetRegistrationsForType(result.MappedTo);
                }
                else if (registrationStore.Contains(result.MappedFrom))
                {
                    actions = registrationStore.GetRegistrationsForType(result.MappedFrom);
                }

                if (actions != null)
                {
                    for (int i = 0; i < actions.Count; i++)
                    {
                        var actionRegistration = actions[i];
                        result.Result = actionRegistration.ResolveWith(new ValueResolver(result.Result), store, null);
                    }
                }

                return result;
            };
        }
    }
}