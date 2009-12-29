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
using System.Linq.Expressions;
using System.Reflection;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class TypeGenerationContext
    {
        internal BuilderBundle Builder { get; private set; }
        internal IList<ITypeGenerationAction> TypeGenerationActions { get; private set; }
        internal Type BaseType { get; private set; }
        internal string TypeName { get; private set; }

        public TypeGenerationContext(BuilderBundle bundle)
        {
            BaseType = typeof(object);
            TypeGenerationActions = new List<ITypeGenerationAction>();
            this.Builder = bundle;
            AddDefaultConstructor();
        }

        public void InheritFrom<TBaseType>()
        {
            BaseType = typeof (TBaseType);
        }

        public void Named(string name)
        {
            TypeName = name;
        }

        internal void AddDefaultConstructor()
        {
            this.TypeGenerationActions.Add(new AddDefaultConstructorAction(this.Builder));
        }

        public void AddMethod(Action<MethodGenerationContext> nestedClosure)
        {
            var context = new MethodGenerationContext(this, nestedClosure);

            context.GeneratedMethod.ReturnFrom(() => context.GeneratedMethod.MethodBuilder.MethodBuilder);
        }

        public void OverrideMethod<TBaseType>(Expression<Action<TBaseType>> expression, Action<OverrideMethodContext> nestedClosure)
        {
            MethodInfo info = ((MethodCallExpression)expression.Body).Method;
            var addMethodAction = new AddMethodAction(this.Builder, () => info.Name, () => info.ReturnType, () => info.GetParameters().Select(p => p.ParameterType).ToArray(), true);
            this.TypeGenerationActions.Add(addMethodAction);
            
            var method = new GeneratedMethod(new MethodBuilderBundle(this.Builder, addMethodAction.MethodBuilder), this.TypeGenerationActions);
            
            var context = new OverrideMethodContext(info, method, this);
            nestedClosure(context);

            method.ReturnFrom(addMethodAction.MethodBuilder);
        }

        public void AddConstructor(Action<ConstructorGenerationContext> nestedClosure)
        {
            var context = new ConstructorGenerationContext();
            nestedClosure(context);

            this.TypeGenerationActions.Add(new AddConstructorAction(this.Builder, context.Arguments.Select(arg => arg.Type).ToList()));
        }
    }
}
