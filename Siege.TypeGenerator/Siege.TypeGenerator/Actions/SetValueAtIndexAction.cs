﻿/*   Copyright 2009 - 2010 Marcus Bratton

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

using System.Reflection.Emit;

namespace Siege.TypeGenerator.Actions
{
    public class SetValueAtIndexAction : ITypeGenerationAction
    {
        private readonly GeneratedArray array;
        private readonly GeneratedMethod method;
        private readonly int index;
        private readonly GeneratedVariable variable;

        public SetValueAtIndexAction(GeneratedMethod method, GeneratedArray array, GeneratedVariable variable, int index)
        {
            this.method = method;
            this.array = array;
            this.variable = variable;
            this.index = index;
        }

        public void Execute()
        {
            ILGenerator generator = method.MethodBuilder().MethodBuilder.GetILGenerator();
            
            generator.Emit(OpCodes.Ldloc, array.LocalIndex);
            generator.Emit(OpCodes.Ldc_I4, index);
            generator.Emit(OpCodes.Ldloc, variable.LocalIndex);
            generator.Emit(OpCodes.Stelem_Ref);
        }
    }
}