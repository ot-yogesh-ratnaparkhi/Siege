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

using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class SetValueOnObjectAction : ITypeGenerationAction
    {
        private readonly GeneratedMethod method;
        private readonly GeneratedVariable variable;
        private readonly object value;
        private readonly MethodInfo info;

        public SetValueOnObjectAction(GeneratedMethod method, GeneratedVariable variable, object value, MethodInfo info)
        {
            this.method = method;
            this.variable = variable;
            this.value = value;
            this.info = info;
        }

        public void Execute()
        {
            var generator = method.MethodBuilder().MethodBuilder.GetILGenerator();

            generator.Emit(OpCodes.Ldloc, variable.LocalIndex);
            
            if(value != null && value is int)
            {
                generator.Emit(OpCodes.Ldc_I4, (int)value);
            }
            else if (value != null && value is string)
            {
                generator.Emit(OpCodes.Ldstr, (string)value);
            }
            else if (value != null && value is MethodParameter)
            {
                generator.Emit(OpCodes.Ldarg, ((MethodParameter)value).Index+1);
            }

            generator.Emit(OpCodes.Callvirt, info);
        }
    }
}
