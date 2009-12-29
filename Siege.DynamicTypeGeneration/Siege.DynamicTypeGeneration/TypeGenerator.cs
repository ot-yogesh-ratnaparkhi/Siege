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
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Siege.DynamicTypeGeneration
{
    public class TypeGenerator
    {
        private static AssemblyBuilder assemblyBuilder;
        
        public TypeGenerator()
        {
            const string dllName = "Siege.DynamicTypes";
            AssemblyName assemblyName = new AssemblyName { Name = dllName };
            AppDomain thisDomain = Thread.GetDomain();

            assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
        }

        public Type CreateType(Action<TypeGenerationContext> nestedClosure)
        {
            
            BuilderBundle bundle = new BuilderBundle
                                       {
                                           ModuleBuilder =
                                               assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name,
                                                                                   assemblyBuilder.GetName().Name +
                                                                                   ".dll")
                                       };

            var context = new TypeGenerationContext(bundle);
            nestedClosure(context);

            if(!context.ConstructorAdded) context.AddDefaultConstructor();

            bundle.TypeBuilder = bundle.ModuleBuilder.DefineType(context.TypeName,
                                                        TypeAttributes.Public |
                                                        TypeAttributes.Class |
                                                        TypeAttributes.AutoClass |
                                                        TypeAttributes.AnsiClass |
                                                        TypeAttributes.BeforeFieldInit |
                                                        TypeAttributes.AutoLayout,
                                                        context.BaseType);

            
            var type = new GeneratedType(bundle, context.TypeGenerationActions);
            var returnType = type.Create();

            assemblyBuilder.Save(assemblyBuilder.GetName().Name + ".dll");

            return returnType;
        }

        //public GeneratedMethod CreateMethod(string methodName, Type returnType, Type[] parameterTypes, bool isOverride)
        //{
        //    var action = new AddMethodAction(this.bundle, methodName, returnType, parameterTypes, isOverride);

        //    var methodBundle = new MethodBuilderBundle(bundle) {MethodBuilder = action.MethodBuilder};
        //    var method = new GeneratedMethod(methodBundle, this.actions);

        //    this.actions.Add(action);
        //    if (returnType != typeof(void)) method.AddLocal(action.MethodBuilder);

        //    return method;
        //}

        //public Type Create()
        //{
        //    foreach(ITypeGenerationAction action in this.actions)
        //    {
        //        action.Execute();
        //    }

        //    Type type = this.bundle.TypeBuilder.CreateType();

        //    assemblyBuilder.Save(assemblyBuilder.GetName().Name + ".dll");

        //    return type;
        //}

        //public GeneratedField CreateField(FieldInfo field)
        //{
        //    var action = new AddFieldAction(this.bundle, field);
        //    var generatedField = new GeneratedField(action);

        //    this.actions.Add(action);

        //    return generatedField;
        //}
    }
}
