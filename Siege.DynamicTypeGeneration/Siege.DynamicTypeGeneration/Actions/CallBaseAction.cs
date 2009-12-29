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
    public class CallBaseAction : CallAction
    {
        private readonly Type baseType;
        private readonly int localIndex;

        public CallBaseAction(MethodBuilderBundle bundle, MethodInfo method, IList<ITypeGenerationAction> actions, Type baseType, GeneratedMethod generatedMethod, int localIndex)
            : base(bundle, method, actions, generatedMethod)
        {
            this.baseType = baseType;
            this.localIndex = localIndex;
        }

        public override void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();

            if (target != null)
            {
                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Ldfld, target);
            }
            else
            {
                methodGenerator.Emit(OpCodes.Ldarg_0);
            }

            List<Type> parameters = new List<Type>();
            var methodParameters = method.GetParameters();
            
            for (int i = 0; i < methodParameters.Length; i++)
            {
                parameters.Add(methodParameters[i].ParameterType);
                methodGenerator.Emit(OpCodes.Ldarg, i+1);
            }

            MethodInfo baseMethod = baseType.GetMethod(method.Name, parameters.ToArray());
            methodGenerator.Emit(OpCodes.Call, baseMethod);

            if (baseMethod.ReturnType != typeof(void)) methodGenerator.Emit(OpCodes.Stloc, localIndex);
        }
    }
}
