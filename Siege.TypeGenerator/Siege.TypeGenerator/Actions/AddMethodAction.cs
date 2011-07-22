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
    internal class AddMethodAction : ITypeGenerationAction
    {
        private readonly Func<Func<Type>> returnType;
        private readonly Func<Type[]> parameterTypes;
        private readonly bool isOverride;
        internal MethodBuilderBundle MethodBuilder { get; private set; }
        private readonly Func<BuilderBundle> builderBundle;
        private readonly Func<Func<string>> methodName;
        private MethodBuilder methodBuilder;

        public AddMethodAction(Func<BuilderBundle> bundle, Func<Func<string>> methodName, Func<Func<Type>> returnType, Func<Type[]> parameterTypes, bool isOverride)
        {
            this.builderBundle = bundle;
            this.methodName = methodName;
            this.returnType = returnType;
            this.parameterTypes = parameterTypes;
            this.isOverride = isOverride;
        }

        public void Execute()
        {
            var methodAttributes = MethodAttributes.Public;
            if (isOverride) methodAttributes |= MethodAttributes.Virtual;
            
            methodBuilder = builderBundle().TypeBuilder.DefineMethod(
                methodName()(),
                methodAttributes,
                returnType()(), parameterTypes());

            int counter = 1;
            foreach (Type type in this.parameterTypes())
            {
                methodBuilder.DefineParameter(counter, ParameterAttributes.None, type.Name + "_" + counter);
                counter++;
            }

            this.MethodBuilder = new MethodBuilderBundle(builderBundle(), methodBuilder)
                                     {TypeBuilder = builderBundle().TypeBuilder};
        }
    }
}