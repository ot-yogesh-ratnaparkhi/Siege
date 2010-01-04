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
using System.Reflection;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class GeneratedVariable : ILocalIndexer
    {
        protected readonly Func<int> localIndex;
        protected IList<ITypeGenerationAction> actions;
        protected readonly GeneratedMethod method;

        public GeneratedVariable(Func<int> localIndex, IList<ITypeGenerationAction> actions, GeneratedMethod method)
        {
            this.localIndex = localIndex;
            this.actions = actions;
            this.method = method;
        }

        public void AssignFrom(Func<ILocalIndexer> item)
        {
            item();
            actions.Add(new VariableAssignmentAction(() => method.MethodBuilder(), localIndex()));
        }

        public Func<int> LocalIndex
        {
            get { return localIndex; }
        }

        public GeneratedVariable Invoke(Type returnType, MethodInfo methodInfo, List<IGeneratedParameter> parameters)
        {
            actions.Add(new VariableLoadAction(method, this.localIndex));
            
            return method.Call(() => methodInfo, () => parameters);
        }
        
        public GeneratedVariable Invoke<TType>(Expression<Action<TType>> expression)
        {
            MethodCallExpression methodCall = expression.Body as MethodCallExpression;
            List<IGeneratedParameter> parameters = new List<IGeneratedParameter>();

            int argCount = 0;
            foreach(ParameterInfo info in methodCall.Method.GetParameters())
            {
                argCount++;
                parameters.Add(new ExpressionParameter(info.ParameterType, argCount));
            }

            return Invoke(typeof(TType), methodCall.Method, parameters);
        }

        public void AssignFrom(GeneratedField field)
        {
            actions.Add(new FieldLoadAction(method, field.Field));
            actions.Add(new VariableAssignmentAction(() => method.MethodBuilder(), localIndex()));
        }

        public MethodInfo GetMethod<TType>(Expression<Action<TType>> expression)
        {
            MethodCallExpression methodCall = expression.Body as MethodCallExpression;
            return methodCall.Method;
        }
    }
}
