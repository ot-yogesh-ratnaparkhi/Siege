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

namespace Siege.Foundry.Actions
{
    internal class CallAction : ITypeGenerationAction, ILocalIndexer
    {
        protected readonly Func<MethodBuilderBundle> bundle;
        private readonly Func<GeneratedMethod> targetMethod;
        private readonly Func<DelegateMethod> delegateMethod;
        protected readonly Func<MethodInfo> method;
        protected IList<ITypeGenerationAction> actions;
        protected readonly GeneratedMethod generatedMethod;
        private readonly Func<List<GeneratedField>> fields;
        protected int localIndex;
        private readonly Func<List<IGeneratedParameter>> parameters;
        private List<ILocalIndexer> variables = new List<ILocalIndexer>();

        public CallAction(Func<MethodBuilderBundle> bundle, Func<MethodInfo> method, IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
            this.method = method;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
        }

        public CallAction(Func<MethodBuilderBundle> bundle, Func<MethodInfo> method, IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod, Func<List<IGeneratedParameter>> parameters)
        {
            this.bundle = bundle;
            this.method = method;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
            this.parameters = parameters;
        }

        public CallAction(Func<MethodBuilderBundle> bundle, Func<MethodInfo> method, IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod, Func<List<GeneratedField>> fields)
        {
            this.bundle = bundle;
            this.method = method;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
            this.fields = fields;
        }

        public CallAction(Func<MethodBuilderBundle> bundle, Func<GeneratedMethod> targetMethod, IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod, Func<List<GeneratedField>> fields)
        {
            this.bundle = bundle;
            this.targetMethod = targetMethod;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
            this.fields = fields;
        }

        public CallAction(Func<MethodBuilderBundle> bundle, Func<DelegateMethod> targetMethod, IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod, Func<List<GeneratedField>> fields)
        {
            this.bundle = bundle;
            this.delegateMethod = targetMethod;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
            this.fields = fields;
        }

        public void WithArgument(ILocalIndexer variable)
        {
            this.variables.Add(variable);
        }

        public virtual void Execute()
        {
            var methodGenerator = bundle().MethodBuilder.GetILGenerator();

            if (fields != null)
            {
                foreach(GeneratedField field in fields())
                {
                    methodGenerator.Emit(OpCodes.Ldarg_0);
                    methodGenerator.Emit(OpCodes.Ldfld, field.Field());
                }
            }

            if (parameters != null)
            {
                foreach(IGeneratedParameter parameter in parameters())
                {
                    methodGenerator.Emit(OpCodes.Ldarg, parameter.LocalIndex);
                }
            }

            foreach(ILocalIndexer variable in variables)
            {
                methodGenerator.Emit(OpCodes.Ldloc, variable.LocalIndex);
            }

            if (method != null) methodGenerator.Emit(OpCodes.Call, method());
            if (targetMethod != null) methodGenerator.Emit(OpCodes.Call, targetMethod().MethodBuilder().MethodBuilder);
            if (delegateMethod != null) methodGenerator.Emit(OpCodes.Call, delegateMethod().Method().MethodBuilder().MethodBuilder);
            localIndex = generatedMethod.LocalCount - 1;
        }

        public int LocalIndex
        {
            get { return localIndex; }
        }
    }
}