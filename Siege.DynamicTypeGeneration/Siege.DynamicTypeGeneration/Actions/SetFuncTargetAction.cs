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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class SetFuncTargetAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        private readonly MethodInfo method;
        private IList<ITypeGenerationAction> actions;
        private readonly GeneratedMethod generatedMethod;

        public SetFuncTargetAction(MethodBuilderBundle bundle, MethodInfo method, IList<ITypeGenerationAction> actions,
                                   GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
            this.method = method;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
        }

        public void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();

            if (generatedMethod.Locals.Where(local => local.Entry == method).Count() > 0)
            {
                methodGenerator.Emit(OpCodes.Ldloc, generatedMethod.Locals.Where(local => local.Entry == method).First().Index);
            }
            else
            {
                methodGenerator.Emit(OpCodes.Ldarg_0);
            }
            methodGenerator.Emit(OpCodes.Ldftn, method);
        }

        public CallAction Call(MethodInfo method)
        {
            var action = new CallAction(bundle, method, actions, generatedMethod);
            this.actions.Add(action);
            return action;
        }
    }
}