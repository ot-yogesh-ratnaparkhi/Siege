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
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class ReturnAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        protected readonly Func<MethodBuilder> method;
        private readonly GeneratedMethod generatedMethod;

        public ReturnAction(MethodBuilderBundle bundle, Func<MethodBuilder> method, GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
            this.method = method;
            this.generatedMethod = generatedMethod;
        }

        public virtual void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();
            MethodInfo info = method();
            if (info.ReturnType != typeof(void))
            {
                methodGenerator.Emit(OpCodes.Ldloc, generatedMethod.LocalCount-1);

                //if (generatedMethod.LocalCount > 1)
                //{
                //    methodGenerator.Emit(OpCodes.Stloc_0);
                //    methodGenerator.Emit(OpCodes.Ldloc_0);
                //}
            }

            methodGenerator.Emit(OpCodes.Ret);
        }
    }
}
