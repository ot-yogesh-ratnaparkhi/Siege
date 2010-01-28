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

namespace Siege.DynamicTypeGeneration
{
    public class DelegateBodyContext
    {
        private readonly MethodBodyContext context;
        private readonly DelegateGenerator generator;
        private int nestedCounter;

        public DelegateBodyContext(MethodBodyContext context, DelegateGenerator generator)
        {
            this.context = context;
            this.generator = generator;
        }

        public MethodInfo Target<TTarget>(Expression<Action<TTarget>> expression)
        {
            MethodCallExpression methodCall = expression.Body as MethodCallExpression;

            List<IGeneratedParameter> parameters = new List<IGeneratedParameter>();

            int counter = 1;
            generator.Returns(methodCall.Method.ReturnType);
            foreach (ParameterInfo parameter in methodCall.Method.GetParameters())
            {
                generator.WithArgument(parameter.ParameterType);
                parameters.Add(new ExpressionParameter(parameter.ParameterType, counter));

                counter++;
            }

            Target(null, parameters, methodCall.Method.Name, () => methodCall.Method);

            return methodCall.Method;
        }

        public MethodInfo Target<TTarget>(GeneratedVariable variable, Expression<Action<TTarget>> expression)
        {
            MethodCallExpression methodCall = expression.Body as MethodCallExpression;

            List<IGeneratedParameter> parameters = new List<IGeneratedParameter>();

            int counter = 1;
            generator.Returns(methodCall.Method.ReturnType);
            foreach (ParameterInfo parameter in methodCall.Method.GetParameters())
            {
                generator.WithArgument(parameter.ParameterType);
                parameters.Add(new ExpressionParameter(parameter.ParameterType, counter));

                counter++;
            }

            Target(variable, parameters, methodCall.Method.Name, () => methodCall.Method);

            return methodCall.Method;
        }

        public MethodInfo Target(MethodInfo info)
        {
            List<IGeneratedParameter> parameters = new List<IGeneratedParameter>();

            int counter = 1;
            generator.Returns(info.ReturnType);
            foreach (ParameterInfo parameter in info.GetParameters())
            {
                generator.WithArgument(parameter.ParameterType);
                parameters.Add(new ExpressionParameter(parameter.ParameterType, counter));

                counter++;
            }

            Target(null, parameters, info.Name, () => info);

            return info;
        }

        public void Target(GeneratedVariable variable, List<IGeneratedParameter> parameters, string methodName, Func<MethodInfo> info)
        {
            generator.AddMethod(methodName, context.WrapMethod(info, parameters));
        }

        public GeneratedDelegate CreateNestedLambda(Action<MethodBodyContext> closure)
        {
            nestedCounter++;
            generator.AddMethod("Lambda_" + nestedCounter, () => this.context.GeneratedMethod, closure);

            return new GeneratedDelegate(this.context, this.context.GeneratedMethod, this.generator);
        }
    }
}
