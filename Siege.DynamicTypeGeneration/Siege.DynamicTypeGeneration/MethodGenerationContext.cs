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
using System.Linq;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class MethodGenerationContext : BaseMethodGenerationContext
    {
        internal Func<List<IGeneratedParameter>> ParameterTypes { get { return () => parameters; } }
        internal Func<string> Name { get; private set; }
        internal Func<Type> ReturnType { get; private set; }

        private List<IGeneratedParameter> parameters = new List<IGeneratedParameter>();
        private int argCount;
        private AddMethodAction addMethodAction;

        public MethodGenerationContext(BaseTypeGenerationContext typeGenerationContext, Action<MethodGenerationContext> closure) : base(typeGenerationContext)
        {
            addMethodAction = new AddMethodAction(typeGenerationContext.Builder, () => Name, () => ReturnType, GetParameters, false);
            typeGenerationContext.TypeGenerationActions.Add(addMethodAction);

            var method = new GeneratedMethod(() => addMethodAction.MethodBuilder, typeGenerationContext.TypeGenerationActions);
            SetMethod(method);
            
            closure(this);
        }

        private Type[] GetParameters()
        {
            List<Type> types = new List<Type>();
            for(int i = 0; i < ParameterTypes().Count(); i++)
            {
                types.Add(ParameterTypes()[i].Type);
            }
            return types.ToArray();
        }

        public void Named(string name)
        {
            Name = () => name;
        }

        public void Named(Func<string> name)
        {
            Name = name;
        }

        public void Returns(Type type)
        {
            ReturnType = () => type;
        }

        public void Returns(Func<Type> type)
        {
            ReturnType = type;
        }

        public IGeneratedParameter CreateArgument<TArgument>()
        {
            return CreateArgument(typeof(TArgument));
        }

        public IGeneratedParameter CreateArgument(Type argumentType)
        {
            argCount++;

            var parameter = new GeneratedParameter(argumentType, argCount, typeGenerationContext, () => addMethodAction.MethodBuilder.MethodBuilder.GetILGenerator);

            parameters.Add(parameter);

            return parameter;
        }

        public void AddArguments(Func<List<IGeneratedParameter>> parameters)
        {
            foreach(IGeneratedParameter parameter in parameters())
            {
                this.parameters.Add(parameter);
            }
        }
    }
}
