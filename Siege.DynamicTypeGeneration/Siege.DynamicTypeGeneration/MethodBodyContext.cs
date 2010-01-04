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
using System.Reflection;

namespace Siege.DynamicTypeGeneration
{
    public class MethodBodyContext
    {
        protected readonly GeneratedMethod method;
        protected readonly TypeGenerationContext typeGenerationContext;
        private readonly BaseMethodGenerationContext context;

        public MethodBodyContext(GeneratedMethod method, TypeGenerationContext typeGenerationContext, BaseMethodGenerationContext context)
        {
            this.method = method;
            this.typeGenerationContext = typeGenerationContext;
            this.context = context;
        }

        public Func<ILocalIndexer> Instantiate<TType>()
        {
            return Instantiate(typeof (TType));
        }

        public Func<ILocalIndexer> Instantiate(Type type)
        {
            return () => new LocalIndexer(method.Instantiate(type, new Type[0]));
        }

        public GeneratedVariable CreateVariable<TVariable>()
        {
            method.AddLocal(typeof(TVariable));

            return new GeneratedVariable(() => method.LocalCount() - 1, typeGenerationContext.TypeGenerationActions, method);
        }

        public void TargettingSelf()
        {
            method.TargettingSelf();
        }

        public GeneratedVariable CallBase(MethodInfo info)
        {
            return method.CallBase(info, typeGenerationContext.BaseType);
        }

        public GeneratedVariable Call(Func<MethodInfo> info, Func<List<IGeneratedParameter>> parameters)
        {
            return method.Call(info, parameters);
        }

        public void Return(ILocalIndexer index)
        {
            context.ReturnDeclared = true;
            method.Return(() => index);
        }

        public void Return()
        {
            context.ReturnDeclared = true;
            method.ReturnFrom();
        }

        public GeneratedVariable CreateDelegate<TType>(Action<DelegateBodyContext> closure)
        {
            closure(new DelegateBodyContext(typeGenerationContext, this.method));
            method.CreateDelegate(typeof(TType)); 

            var variable = new GeneratedVariable(() => method.LocalCount() - 1, typeGenerationContext.TypeGenerationActions, method);
            variable.AssignFrom(() => new LocalIndexer(method.LocalCount() - 1));
            return variable;
        }
    }
}
