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
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class AddConstructorAction : ITypeGenerationAction
    {
        private readonly BuilderBundle builder;
        private readonly Func<List<Type>> types;
        private ConstructorBuilder constructorBuilder;
        public ConstructorBuilder Constructor { get { return constructorBuilder; } }
        public Func<ILGenerator> Builder { get { return () => constructorBuilder.GetILGenerator(); } }

        public AddConstructorAction(BuilderBundle bundle, Func<List<Type>> types)
        {
            builder = bundle;
            this.types = types;
        }

        public void Execute()
        {
            constructorBuilder = builder.TypeBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                this.types().ToArray());
        }
    }
}