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

namespace Siege.TypeGenerator.Actions
{
    internal class WrapMethodAction : ITypeGenerationAction
    {
        public GeneratedMethod GeneratedMethod { get; private set; }

        public WrapMethodAction(BaseTypeGenerationContext context, Func<MethodBuilderBundle> methodBuilder, List<IGeneratedParameter> parameters, int index)
        {
            var generatedMethod = context.AddMethod(method =>
            {
                method.Named(() => methodBuilder().MethodBuilder.Name + "_" + index);
                method.AddArguments(() => parameters);
                method.Returns(() => methodBuilder().MethodBuilder.ReturnType);

                method.WithBody(body =>
                {
                    body.TargettingSelf();
                    body.Return(body.Call(() => methodBuilder().MethodBuilder, () => parameters));
                });
            });

            GeneratedMethod = generatedMethod;
        }

        public WrapMethodAction(BaseTypeGenerationContext context, Func<MethodInfo> methodInfo, List<IGeneratedParameter> parameters, int index)
        {
            var generatedMethod = context.AddMethod(method =>
            {
                method.Named(() => methodInfo().Name + "_" + index);
                method.AddArguments(() => parameters);
                method.Returns(() => methodInfo().ReturnType);

                method.WithBody(body =>
                {
                    body.TargettingSelf();
                    if (method.ReturnType() != typeof(void))
                    {
                        var variable = body.CreateVariable(method.ReturnType());

                        variable.AssignFrom(() => body.Call(methodInfo, () => parameters));
                        body.Return(variable);
                    }
                    else
                    {
                        body.Call(methodInfo, () => parameters);
                    }
                });
            });

            GeneratedMethod = generatedMethod;
        }

        public void Execute()
        {

        }
    }
}
