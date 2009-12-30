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
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class GeneratedParameter
    {
        private readonly TypeGenerationContext context;
        private readonly Func<Func<ILGenerator>> builder;
        public int Index { get; private set; }
        public Type Type { get; private set; }

        public GeneratedParameter(Type type, int parameterIndex, TypeGenerationContext context, Func<Func<ILGenerator>> builder)
        {
            this.context = context;
            this.builder = builder;
            Type = type;
            Index = parameterIndex;
        }

        public void AssignTo(GeneratedField field)
        {
            var action = new FieldAssignmentAction(builder, this);
            action.To(field.Field);
            this.context.TypeGenerationActions.Add(action);
        }
    }
}
