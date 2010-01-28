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
using System.Reflection;
using System.Reflection.Emit;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class MethodBodyContext
    {
        internal GeneratedMethod GeneratedMethod { get; set; }
        internal BaseTypeGenerationContext TypeGenerationContext { get; private set; }
        internal BaseMethodGenerationContext MethodContext { get; private set; }
        private int index;
        private DelegateGenerator generator;

        public MethodBodyContext(GeneratedMethod method, BaseTypeGenerationContext typeGenerationContext, BaseMethodGenerationContext context)
        {
            this.GeneratedMethod = method;
            this.TypeGenerationContext = typeGenerationContext;
            this.MethodContext = context;
            this.generator = new DelegateGenerator(typeGenerationContext);
        }

        public Func<ILocalIndexer> Instantiate<TType>()
        {
            return Instantiate(typeof (TType));
        }

        public Func<ILocalIndexer> Instantiate(Type type)
        {
            return Instantiate(type, new Type[0]);
        }

        public Func<ILocalIndexer> Instantiate(Func<ConstructorBuilder> type)
        {
            return () => new LocalIndexer(GeneratedMethod.Instantiate(type));
        }

        public Func<ILocalIndexer> Instantiate(Type type, Type[] constructorArguments)
        {
            return () => new LocalIndexer(GeneratedMethod.Instantiate(type, constructorArguments));
        }

        public Func<ILocalIndexer> Instantiate(Func<BuilderBundle> type, Type[] constructorArguments)
        {
            return () => new LocalIndexer(GeneratedMethod.Instantiate(type, constructorArguments));
        }

        public GeneratedVariable CreateVariable(Type variableType)
        {
            GeneratedMethod.AddLocal(variableType);

            return new GeneratedVariable(variableType, GeneratedMethod.LocalCount - 1, TypeGenerationContext.TypeGenerationActions, GeneratedMethod);
        }

        public GeneratedVariable CreateVariable(Func<BuilderBundle> variableType)
        {
            GeneratedMethod.AddLocal(variableType);

            return new GeneratedVariable(variableType, GeneratedMethod.LocalCount - 1, TypeGenerationContext.TypeGenerationActions, GeneratedMethod);
        }

        public GeneratedVariable CreateVariable(Func<Type> variableType)
        {
            GeneratedMethod.AddLocal(variableType);

            return new GeneratedVariable(variableType, GeneratedMethod.LocalCount - 1, TypeGenerationContext.TypeGenerationActions, GeneratedMethod);
        }

        public GeneratedVariable CreateVariable<TVariable>()
        {
            return CreateVariable(typeof (TVariable));
        }

        public void TargettingSelf()
        {
            GeneratedMethod.TargettingSelf();
        }

        public void Target(GeneratedVariable variable)
        {
            this.TypeGenerationContext.TypeGenerationActions.Add(new VariableLoadAction(GeneratedMethod, variable.LocalIndex));
        }

        public ILocalIndexer CallBase(MethodInfo info)
        {
            return GeneratedMethod.CallBase(info, TypeGenerationContext.BaseType);
        }

        public ILocalIndexer Call(Func<MethodInfo> info, Func<List<IGeneratedParameter>> parameters)
        {
            return GeneratedMethod.Call(info, parameters);
        }

        public ILocalIndexer Call(GeneratedMethod info, Type returnType)
        {
            return GeneratedMethod.Call(() => info, returnType);
        }

        public ILocalIndexer Call(Func<MethodInfo> info, Func<List<GeneratedField>> fields)
        {
            return GeneratedMethod.Call(info, fields);
        }

        public ILocalIndexer Call(Func<DelegateMethod> info, Func<List<GeneratedField>> fields)
        {
            return GeneratedMethod.Call(info, fields);
        }

        public ILocalIndexer Call(Func<GeneratedMethod> info, Func<List<GeneratedField>> fields)
        {
            return GeneratedMethod.Call(info, fields);
        }

        public void Return(ILocalIndexer index)
        {
            MethodContext.ReturnDeclared = true;
            GeneratedMethod.Return(() => index);
        }

        public void Return()
        {
            MethodContext.ReturnDeclared = true;
            GeneratedMethod.ReturnFrom();
        }

        public GeneratedDelegate CreateLambda(Action<DelegateBodyContext> closure)
        {
            closure(new DelegateBodyContext(this, generator));
            generator.Build();

            return new GeneratedDelegate(this, this.GeneratedMethod, generator);
        }

        public Func<GeneratedMethod> WrapMethod(Func<MethodInfo> methodInfo, List<IGeneratedParameter> parameters)
        {
            index++;
            var action = new WrapMethodAction(this.TypeGenerationContext, methodInfo, parameters, index);
            this.TypeGenerationContext.TypeGenerationActions.Add(action);

            return () => action.GeneratedMethod;
        }
    }
}
