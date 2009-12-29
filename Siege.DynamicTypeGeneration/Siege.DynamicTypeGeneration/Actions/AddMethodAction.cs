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
    public class AddMethodAction : ITypeGenerationAction
    {
        private readonly Func<Type> returnType;
        private readonly Func<Type[]> parameterTypes;
        private readonly bool isOverride;
        private MethodBuilderBundle bundle;
        private readonly BuilderBundle builderBundle;
        private readonly Func<string> methodName;
        private MethodBuilder methodBuilder;

        public AddMethodAction(BuilderBundle bundle, Func<string> methodName, Func<Type> returnType, Func<Type[]> parameterTypes, bool isOverride)
        {
            this.builderBundle = bundle;
            this.methodName = methodName;
            this.returnType = returnType;
            this.parameterTypes = parameterTypes;
            this.isOverride = isOverride;
        }

        public Func<MethodBuilder> MethodBuilder { get { return () => bundle.MethodBuilder; } }

        public void Execute()
        {
            var methodAttributes = MethodAttributes.Public;
            if (isOverride) methodAttributes |= MethodAttributes.Virtual;
            
            methodBuilder = builderBundle.TypeBuilder.DefineMethod(
                methodName(),
                methodAttributes,
                returnType(), parameterTypes());

            this.bundle = new MethodBuilderBundle(builderBundle, () => methodBuilder);
        }
    }
}