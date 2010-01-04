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
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    internal class CallAction : ITypeGenerationAction, ILocalIndexer
    {
        protected readonly Func<MethodBuilderBundle> bundle;
        protected readonly Func<MethodInfo> method;
        protected IList<ITypeGenerationAction> actions;
        protected readonly GeneratedMethod generatedMethod;
        protected int localIndex;
        private readonly Func<List<IGeneratedParameter>> parameters;
        protected FieldInfo target;

        public CallAction(Func<MethodBuilderBundle> bundle, Func<MethodInfo> method, IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod, Func<List<IGeneratedParameter>> parameters)
        {
            this.bundle = bundle;
            this.method = method;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
            this.parameters = parameters;
        }

        public void On(FieldInfo field)
        {
            target = field;
        }

        public virtual void Execute()
        {
            var methodGenerator = bundle().MethodBuilder.GetILGenerator();

            if (method().ReturnType != typeof(void))
            {
                methodGenerator.DeclareLocal(method().ReturnType);

                generatedMethod.AddLocal(new Local { Entry = method().ReturnType, Index = generatedMethod.LocalCount() });
            }

            if (target != null)
            {
                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Ldfld, target);
            }

            if (parameters != null)
            {
                foreach(IGeneratedParameter parameter in parameters())
                {
                    methodGenerator.Emit(OpCodes.Ldarg, parameter.LocalIndex());
                }
            }

            methodGenerator.Emit(OpCodes.Call, method());
            localIndex = generatedMethod.LocalCount() - 1;
            if (method().ReturnType != typeof(void)) methodGenerator.Emit(OpCodes.Stloc, localIndex);
        }

        public Func<int> LocalIndex
        {
            get { return () => localIndex; }
        }
    }
}