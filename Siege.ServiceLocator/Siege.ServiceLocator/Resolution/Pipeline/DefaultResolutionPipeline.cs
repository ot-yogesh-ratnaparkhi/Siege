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

using Siege.ServiceLocator.InternalStorage;

namespace Siege.ServiceLocator.Resolution.Pipeline
{
    public class DefaultResolutionPipeline : FactoryResolutionPipeline
    {
        public DefaultResolutionPipeline(Foundation foundation, IServiceLocatorAdapter adapter, IServiceLocatorStore store) : base(foundation, adapter, store)
        {
            var pipeline = new PostResolutionPipeline(foundation, adapter, store);

            Add(new AdapterPipelineItem(foundation, adapter, store, pipeline));
        }
    }
}