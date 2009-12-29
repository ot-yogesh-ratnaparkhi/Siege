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
using System.Reflection;

namespace Siege.DynamicTypeGeneration
{
    public class MethodBodyContext
    {
        protected readonly GeneratedMethod method;
        protected readonly TypeGenerationContext typeGenerationContext;

        public MethodBodyContext(GeneratedMethod method, TypeGenerationContext typeGenerationContext)
        {
            this.method = method;
            this.typeGenerationContext = typeGenerationContext;
        }

        public Func<int> Instantiate<TType>()
        {
            return () => this.method.Instantiate(typeof(TType), new Type[0]);
        }

        public GeneratedVariable<TVariable> CreateVariable<TVariable>()
        {
            method.AddLocal(typeof(TVariable));

            return new GeneratedVariable<TVariable>(method.LocalCount - 1, typeGenerationContext.TypeGenerationActions, method);
        }

        public void CallBase(MethodInfo info)
        {
            method.CallBase(info, typeGenerationContext.BaseType);
        }
    }
}
