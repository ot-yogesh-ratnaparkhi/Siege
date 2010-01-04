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
        internal bool ConstructorAdded { get; private set; }

        public TypeGenerationContext(BuilderBundle bundle, Action<TypeGenerationContext> nestedClosure)
        {
            BaseType = typeof(object);
            TypeGenerationActions = new List<ITypeGenerationAction>();

            var defineTypeAction = new DefineTypeAction(bundle, () => TypeName, () => BaseType);
            this.TypeGenerationActions.Add(defineTypeAction);

            bundle.TypeBuilderDelegate = () => defineTypeAction.TypeBuilder;
            this.Builder = bundle;

            nestedClosure(this);
            if (!ConstructorAdded) AddDefaultConstructor();
        }

        public void InheritFrom<TBaseType>() where TBaseType : class
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
            ConstructorAdded = true;
        }

        public GeneratedMethod AddMethod(Action<MethodGenerationContext> nestedClosure)
        {
            var context = new MethodGenerationContext(this, nestedClosure);

            if(!context.ReturnDeclared) context.GeneratedMethod.ReturnFrom();

            return context.GeneratedMethod;
        }

        public void OverrideMethod<TBaseType>(Expression<Action<TBaseType>> expression, Action<OverrideMethodContext> nestedClosure)
        {
            OverrideMethod(((MethodCallExpression)expression.Body).Method, nestedClosure);
        }

        public void OverrideMethod(MethodInfo info, Action<OverrideMethodContext> nestedClosure)
        {
            var addMethodAction = new AddMethodAction(this.Builder, () => () => info.Name, () => () => info.ReturnType, () => info.GetParameters().Select(p => p.ParameterType).ToArray(), true);
            this.TypeGenerationActions.Add(addMethodAction);

            var method = new GeneratedMethod(() => addMethodAction.MethodBuilder, this.TypeGenerationActions);

            var context = new OverrideMethodContext(info, method, this);
            nestedClosure(context);

            if (!context.ReturnDeclared) method.ReturnFrom();
        }

        public void AddConstructor(Action<ConstructorGenerationContext> nestedClosure)
        {
            ConstructorAdded = true;
            new ConstructorGenerationContext(this, nestedClosure);
        }

        public GeneratedField AddField<TFieldType>(string fieldName)
        {
            return AddField(typeof (TFieldType), fieldName);
        }

        public GeneratedField AddField(Type type, string fieldName)
        {
            var action = new AddFieldAction(this.Builder, () => fieldName, () => type);
            this.TypeGenerationActions.Add(action);

            return new GeneratedField(action);
        }
    }
}
