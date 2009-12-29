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
using System.Linq.Expressions;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class GeneratedVariable<TType>
    {
        private readonly int localIndex;
        private IList<ITypeGenerationAction> actions;
        private readonly GeneratedMethod method;

        public GeneratedVariable(int localIndex, IList<ITypeGenerationAction> actions, GeneratedMethod method)
        {
            this.localIndex = localIndex;
            this.actions = actions;
            this.method = method;
        }

        public void AssignFrom(Func<int> item)
        {
            item();
            actions.Add(new VariableAssignmentAction(method.MethodBuilder, localIndex));
        }

        public void Invoke(Expression<Action<TType>> expression)
        {
            actions.Add(new VariableLoadAction(method, localIndex));
            MethodCallExpression methodCall = expression.Body as MethodCallExpression;
            method.Call(methodCall.Method);
        }
    }
}
