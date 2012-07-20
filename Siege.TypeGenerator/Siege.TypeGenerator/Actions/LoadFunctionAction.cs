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

namespace Siege.TypeGenerator.Actions
{
    internal class LoadFunctionAction : ITypeGenerationAction
    {
        private readonly Func<GeneratedMethod> method;
        private readonly MethodInfo methodInfo;
        private readonly Func<GeneratedMethod> targetMethod;

        public LoadFunctionAction(Func<GeneratedMethod> method, Func<GeneratedMethod> targetMethod)
        {
            this.method = method;
            this.targetMethod = targetMethod;
        }

        public LoadFunctionAction(Func<GeneratedMethod> method, MethodInfo methodInfo)
        {
            this.method = method;
            this.methodInfo = methodInfo;
        }

        public void Execute()
        {
            ILGenerator generator = method().MethodBuilder().MethodBuilder.GetILGenerator();
            //generator.Emit(OpCodes.Ldarg_0);
            
            if(targetMethod != null) generator.Emit(OpCodes.Ldftn, targetMethod().MethodBuilder().MethodBuilder);
            if (methodInfo != null) generator.Emit(OpCodes.Ldftn, methodInfo);
            
        }
    }
}
