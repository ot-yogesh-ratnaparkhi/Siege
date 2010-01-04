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
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class DelegateBodyContext
    {
        private readonly TypeGenerationContext context;
        private readonly GeneratedMethod method;

        public DelegateBodyContext(TypeGenerationContext context, GeneratedMethod method)
        {
            this.context = context;
            this.method = method;
        }

        public Func<GeneratedMethod> WrappingMethod(MethodInfo methodInfo)
        {
            var action = new WrapMethodAction(context, () => methodInfo, GetParameters(methodInfo));
            context.TypeGenerationActions.Add(action);

            return () => action.GeneratedMethod;
        }

        public Func<GeneratedMethod> WrappingMethod(Func<GeneratedMethod> methodInfo, MethodInfo method)
        {
            var action = new WrapMethodAction(context, methodInfo().MethodBuilder, GetParameters(method));
            context.TypeGenerationActions.Add(action);

            return () => action.GeneratedMethod;
        }


        private List<IGeneratedParameter> GetParameters(MethodInfo info)
        {
            var parameters = new List<IGeneratedParameter>();
            int counter = 1;
            foreach (ParameterInfo parameterInfo in info.GetParameters())
            {
                parameters.Add(new ExpressionParameter(parameterInfo.ParameterType, counter));
                counter++;
            }

            return parameters;
        }

        public void Target(Func<GeneratedMethod> generatedMethod)
        {
            method.Target(generatedMethod);
        }

        public void Target(GeneratedVariable variable, MethodInfo info)
        {
            method.Target(variable, info);
        }
    }
}
