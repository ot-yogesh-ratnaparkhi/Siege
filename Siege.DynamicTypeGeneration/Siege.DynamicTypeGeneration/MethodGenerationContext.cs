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
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class MethodGenerationContext : BaseMethodGenerationContext
    {
        internal List<Type> ParameterTypes { get; private set; }
        internal string Name { get; private set; }
        internal Type ReturnType { get; private set; }

        public MethodGenerationContext(TypeGenerationContext typeGenerationContext, Action<MethodGenerationContext> closure) : base(typeGenerationContext)
        {
            ParameterTypes = new List<Type>();

            var addMethodAction = new AddMethodAction(typeGenerationContext.Builder, () => Name, () => ReturnType, () => ParameterTypes.ToArray(), false);
            typeGenerationContext.TypeGenerationActions.Add(addMethodAction);

            var method = new GeneratedMethod(new MethodBuilderBundle(typeGenerationContext.Builder, addMethodAction.MethodBuilder), typeGenerationContext.TypeGenerationActions);
            SetMethod(method);
            
            closure(this);
        }

        public void Named(string name)
        {
            Name = name;
        }

        public void Returns(Type type)
        {
            ReturnType = type;
        }

        public void AddParameterType(Type parameterType)
        {
            ParameterTypes.Add(parameterType);
        }
    }
}
