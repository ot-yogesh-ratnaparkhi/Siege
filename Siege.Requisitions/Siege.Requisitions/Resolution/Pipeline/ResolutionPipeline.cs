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
using System.Linq;
using Siege.Requisitions.Exceptions;
using Siege.Requisitions.InternalStorage;

namespace Siege.Requisitions.Resolution.Pipeline
{
    public interface IResolutionPipeline
    {
        List<IPipelineItem> Items { get; }
    }

    public class ResolutionPipeline : IResolutionPipeline
    {
        private readonly List<IPipelineItem> items = new List<IPipelineItem>();
        private readonly List<Type> addedItems = new List<Type>();

        public ResolutionPipeline(Foundation foundation, IServiceLocatorAdapter adapter, IServiceLocatorStore store)
        {
        }

        public object Execute(Type type)
        {
            PipelineResult value = null;
            value = items.Aggregate(value, (current, item) => item.Execute(type, current));

            if (value == null) throw new RegistrationNotFoundException(type);

            return value.Result;
        }

        public void Add(IPipelineItem item)
        {
            if (!this.addedItems.Contains(item.GetType()))
            {
                this.items.Add(item);
                this.addedItems.Add(item.GetType());
            }
        }

        public void Add(IResolutionPipeline pipeline)
        {
            foreach(var item in pipeline.Items)
            {
                Add(item);
            }
        }

        public List<IPipelineItem> Items
        {
            get { return this.items; }
        }
    }
}