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

using System.Linq;
using Siege.ServiceLocator.Exceptions;
using Siege.ServiceLocator.InternalStorage;

namespace Siege.ServiceLocator.Resolution.Pipeline
{
    public class PostResolutionPipeline : ResolutionPipeline
    {
        public PostResolutionPipeline(Foundation foundation, IServiceLocatorAdapter adapter, IServiceLocatorStore store)
        {
            Add(new ConditionalPostResolutionPipelineItem(foundation, adapter, store));
            Add(new DefaultPostResolutionPipelineItem(foundation, adapter, store));
        }

        public PipelineResult Execute(PipelineResult result)
        {
            var tempValue = result.Result;
            var value = result.Result;

            if (value == null) return result;

            result = Items.Aggregate(result, (current, item) => item.Execute(value.GetType(), current));

            if (value == null) throw new RegistrationNotFoundException(tempValue.GetType());

            return result;
        }
    }
}