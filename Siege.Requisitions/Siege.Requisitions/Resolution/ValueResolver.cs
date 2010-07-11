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

namespace Siege.Requisitions.Resolution
{
    public class ValueResolver : IInstanceResolver
    {
        private readonly object value;

        public ValueResolver(object value)
        {
            this.value = value;
        }

        public object GetInstance(Type type, string key, params IResolutionArgument[] parameters)
        {
            return value;
        }

        public object GetInstance(Type type, params IResolutionArgument[] parameters)
        {
            return value;
        }

        public TService GetInstance<TService>(Type type, params IResolutionArgument[] arguments)
        {
            return (TService)value;
        }

        public TService GetInstance<TService>(string key, params IResolutionArgument[] arguments)
        {
            return (TService)value;
        }

        public TService GetInstance<TService>(params IResolutionArgument[] arguments)
        {
            return (TService)value;
        }

        public bool HasTypeRegistered(Type type)
        {
            return true;
        }
    }
}
