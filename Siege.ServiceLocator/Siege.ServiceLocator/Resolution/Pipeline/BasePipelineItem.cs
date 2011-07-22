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
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.Stores;

namespace Siege.ServiceLocator.Resolution.Pipeline
{
    public abstract class BasePipelineItem : IPipelineItem
    {
        protected readonly Foundation foundation;
        protected readonly IServiceLocatorAdapter serviceLocator;
        protected readonly IServiceLocatorStore store;

        protected BasePipelineItem(Foundation foundation, IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store)
        {
            this.foundation = foundation;
            this.serviceLocator = serviceLocator;
            this.store = store;
        }
        
        public abstract PipelineResult Execute(Type requestedType, PipelineResult result);
    }

    public abstract class BasePipelineItem<TRegistrationStore> : IPipelineItem<TRegistrationStore> where TRegistrationStore : IRegistrationStore
    {
        protected readonly Foundation foundation;
        protected readonly IServiceLocatorAdapter serviceLocator;
        protected readonly IServiceLocatorStore store;

        protected BasePipelineItem(Foundation foundation, IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store)
        {
            this.foundation = foundation;
            this.serviceLocator = serviceLocator;
            this.store = store;
        }

        protected abstract Func<PipelineResult> Invoker(Type requestedType, PipelineResult result);

        public virtual PipelineResult Execute(Type requestedType, PipelineResult result)
        {
            return Invoker(requestedType, result)();
        }

        protected object Resolve(IRegistration registration)
        {
            return registration.ResolveWith(serviceLocator, store, null);
        }
    }
}