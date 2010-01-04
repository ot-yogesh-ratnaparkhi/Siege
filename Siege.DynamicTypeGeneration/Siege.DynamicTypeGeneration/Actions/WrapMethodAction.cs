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

namespace Siege.DynamicTypeGeneration.Actions
{
    internal class WrapMethodAction : ITypeGenerationAction
    {
        public GeneratedMethod GeneratedMethod { get; private set; }

        public WrapMethodAction(TypeGenerationContext context, Func<MethodBuilderBundle> methodBuilder, List<IGeneratedParameter> parameters)
        {
            var generatedMethod = context.AddMethod(method =>
            {
                method.Named(() => methodBuilder().MethodBuilder.Name + "_" + Guid.NewGuid());
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

        public WrapMethodAction(TypeGenerationContext context, Func<MethodInfo> methodInfo, List<IGeneratedParameter> parameters)
        {
            var generatedMethod = context.AddMethod(method =>
            {
                method.Named(() => methodInfo().Name + "_" + Guid.NewGuid());
                method.AddArguments(() => parameters);
                method.Returns(() => methodInfo().ReturnType);

                method.WithBody(body =>
                {
                    body.TargettingSelf();
                    body.Return(body.Call(methodInfo, () => parameters));
                });
            });

            GeneratedMethod = generatedMethod;
        }

        public void Execute()
        {

        }
    }
}
