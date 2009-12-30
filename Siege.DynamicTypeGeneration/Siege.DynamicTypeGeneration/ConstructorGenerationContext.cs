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
    public class ConstructorGenerationContext
    {
        private readonly TypeGenerationContext context;
        private int argCount;
        public List<GeneratedParameter> Arguments { get; private set; }
        private Func<AddConstructorAction> constructorActionDelegate { get { return () => constructorAction; } } 
        private AddConstructorAction constructorAction;

        public ConstructorGenerationContext(TypeGenerationContext context, Action<ConstructorGenerationContext> closure)
        {
            this.Arguments = new List<GeneratedParameter>();

            this.context = context;

            var action = new AddConstructorAction(context.Builder, () => this.Arguments.Select(arg => arg.Type).ToList());
            context.TypeGenerationActions.Add(action);
            constructorAction = action;

            closure(this);
            context.TypeGenerationActions.Add(new ConstructorReturnAction(() => action.Constructor));
        }

        public GeneratedParameter CreateArgument<TArgument>()
        {
            argCount++;
            var parameter = new GeneratedParameter(typeof(TArgument), argCount, this.context, () => constructorActionDelegate().Builder);
            
            Arguments.Add(parameter);
         
            return parameter;
        }

        public void WithBody(Action<ConstructorBodyContext> nestedClosure)
        {
            var constructorBody = new ConstructorBodyContext(this.context);

            nestedClosure(constructorBody);
        }
    }
}
