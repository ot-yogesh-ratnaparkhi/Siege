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
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    internal class InstantiationAction : ITypeGenerationAction
    {
        private readonly Func<MethodBuilderBundle> bundle;
        private readonly Func<ConstructorBuilder> constructor;
        private readonly Func<BuilderBundle> typeBuilder;
        private readonly Type type;
        private readonly Type[] constructorArguments;

        public InstantiationAction(Func<MethodBuilderBundle> bundle, Type type, Type[] constructorArguments)
        {
            this.bundle = bundle;
            this.type = type;
            this.constructorArguments = constructorArguments;
        }

        public InstantiationAction(Func<MethodBuilderBundle> bundle, Func<BuilderBundle> typeBuilder, Type[] constructorArguments)
        {
            this.bundle = bundle;
            this.typeBuilder = typeBuilder;
            this.constructorArguments = constructorArguments;
        }

        public InstantiationAction(Func<MethodBuilderBundle> bundle, Func<ConstructorBuilder> constructor)
        {
            this.bundle = bundle;
            this.constructor = constructor;
        }

        public void Execute()
        {
            ILGenerator generator = this.bundle().MethodBuilder.GetILGenerator();

            if(type != null)
            {
                generator.Emit(OpCodes.Newobj, type.GetConstructor(constructorArguments));
            }
            else if(typeBuilder != null)
            {
                generator.Emit(OpCodes.Newobj, typeBuilder().TypeBuilder.GetConstructor(constructorArguments));
            }
            else if (constructor != null)
            {
                generator.Emit(OpCodes.Newobj, constructor());
            }
        }
    }
}
