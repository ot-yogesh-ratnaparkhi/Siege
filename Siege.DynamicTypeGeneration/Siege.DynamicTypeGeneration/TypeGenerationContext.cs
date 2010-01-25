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
    public class BaseTypeGenerationContext
    {
        internal readonly TypeGenerator typeGenerator;
        internal List<Func<BuilderBundle>> TypesToComplete { get; set; }
        internal Func<BuilderBundle> Builder { get; set; }
        internal IList<ITypeGenerationAction> TypeGenerationActions { get; set; }
        internal Type BaseType { get; set; }
        internal string TypeName { get; set; }
        internal bool ConstructorAdded { get; set; }

        public BaseTypeGenerationContext(TypeGenerator generator)
        {
            typeGenerator = generator;
            TypesToComplete = new List<Func<BuilderBundle>>();
        }

        public void InheritFrom<TBaseType>() where TBaseType : class
        {
            InheritFrom(typeof(TBaseType));
        }

        public void InheritFrom(Type baseType)
        {
            BaseType = baseType;
        }

        public void Named(string name)
        {
            TypeName = name;
        }

        internal void AddDefaultConstructor()
        {
            TypeGenerationActions.Add(new AddDefaultConstructorAction(Builder));
            ConstructorAdded = true;
        }

        public GeneratedMethod AddMethod(Action<MethodGenerationContext> nestedClosure)
        {
            var context = new MethodGenerationContext(this, nestedClosure);

            if (!context.ReturnDeclared) context.GeneratedMethod.ReturnFrom();

            return context.GeneratedMethod;
        }

        public void OverrideMethod<TBaseType>(Expression<Action<TBaseType>> expression, Action<OverrideMethodContext> nestedClosure)
        {
            OverrideMethod(((MethodCallExpression)expression.Body).Method, nestedClosure);
        }

        public void OverrideMethod(MethodInfo info, Action<OverrideMethodContext> nestedClosure)
        {
            var addMethodAction = new AddMethodAction(Builder, () => () => info.Name, () => () => info.ReturnType, () => info.GetParameters().Select(p => p.ParameterType).ToArray(), true);
            TypeGenerationActions.Add(addMethodAction);

            var method = new GeneratedMethod(() => addMethodAction.MethodBuilder, TypeGenerationActions);

            var context = new OverrideMethodContext(info, method, this);
            nestedClosure(context);

            if (!context.ReturnDeclared) method.ReturnFrom();
        }

        public AddConstructorAction AddConstructor(Action<ConstructorGenerationContext> nestedClosure)
        {
            ConstructorAdded = true;
            var context =  new ConstructorGenerationContext(this, nestedClosure);
            
            return context.constructorAction;
        }

        public GeneratedField AddField<TFieldType>(string fieldName)
        {
            return AddField(typeof(TFieldType), fieldName);
        }

        public GeneratedField AddField(Type type, string fieldName)
        {
            var action = new AddFieldAction(Builder, () => fieldName, () => type);
            TypeGenerationActions.Add(action);

            return new GeneratedField(type, action);
        }

        public GeneratedField AddField(Func<BuilderBundle> type, string fieldName)
        {
            var action = new AddFieldAction(Builder, () => fieldName, type);
            TypeGenerationActions.Add(action);

            return new GeneratedField(type, action);
        }

        public NestedTypeGenerationContext CreateNestedType(Action<BaseTypeGenerationContext> nestedClosure)
        {
            return new NestedTypeGenerationContext(typeGenerator, Builder, nestedClosure, TypeGenerationActions);
        }

    }

    public class TypeGenerationContext : BaseTypeGenerationContext
    {
        public TypeGenerationContext(TypeGenerator generator, Func<BuilderBundle> bundle, Action<TypeGenerationContext> nestedClosure) : base(generator)
        {
            BaseType = typeof(object);
            TypeGenerationActions = new List<ITypeGenerationAction>();

            var defineTypeAction = new DefineTypeAction(bundle(), () => TypeName, () => BaseType);
            TypeGenerationActions.Add(defineTypeAction);

            Builder = bundle;

            nestedClosure(this);
            if (!ConstructorAdded) AddDefaultConstructor();
        }
    }
}
