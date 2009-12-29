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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CreateFuncAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        private IList<ITypeGenerationAction> actions;
        private readonly GeneratedMethod generatedMethod;
        private ConstructorInfo funcConstructor;
        private static Hashtable generatedTypes = new Hashtable();

        public CreateFuncAction(MethodBuilderBundle bundle, Type returnType, IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
            this.actions = actions;
            this.generatedMethod = generatedMethod;

            Type funcType;

            if (returnType == typeof(void))
            {
                funcType = typeof(Action);
            }
            else
            {
                funcType = typeof(Func<>).MakeGenericType(returnType);
            }

            this.funcConstructor = funcType.GetConstructor(new[] { typeof(object), typeof(IntPtr) });
			
        }

        public SetFuncTargetAction Targetting(MethodInfo method)
        {
			CreateFuncEncapsulationAction nestedType;

			if (!generatedTypes.ContainsKey(bundle.TypeBuilder.Name))
			{
				nestedType = new CreateFuncEncapsulationAction(bundle, method, this.actions, generatedMethod);
				generatedTypes.Add(bundle.TypeBuilder.Name, nestedType);
				this.actions.Add(nestedType);
			}
			else
			{
				nestedType = (CreateFuncEncapsulationAction)generatedTypes[bundle.TypeBuilder.Name];
			}

            this.actions.Add(new LoadNestedTypeAction(bundle, nestedType, generatedMethod));
            var action = new SetFuncTargetAction(this.bundle, nestedType.Method, this.actions, generatedMethod);
            this.actions.Add(action);
            this.actions.Add(this);


            return action;
        }

        public void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();

            methodGenerator.Emit(OpCodes.Newobj, funcConstructor);
        }

        private class LoadNestedTypeAction : ITypeGenerationAction
        {
            private GeneratedMethod generatedMethod;
            private readonly MethodBuilderBundle bundle;
            private readonly CreateFuncEncapsulationAction action;

            public LoadNestedTypeAction(MethodBuilderBundle bundle, CreateFuncEncapsulationAction action, GeneratedMethod generatedMethod)
            {
                this.generatedMethod = generatedMethod;
                this.bundle = bundle;
                this.action = action;
            }

            public void Execute()
            {
                var methodGenerator = bundle.MethodBuilder.GetILGenerator();

				methodGenerator.Emit(OpCodes.Newobj, action.Constructor);
                methodGenerator.Emit(OpCodes.Stloc, generatedMethod.Locals.Where(local => local.Entry == action.Method).First().Index);
                methodGenerator.Emit(OpCodes.Ldloc, generatedMethod.Locals.Where(local => local.Entry == action.Method).First().Index);

                int counter = 1;
                foreach(FieldInfo info in action.PublicFields)
                {
                    methodGenerator.Emit(OpCodes.Ldarg, counter);
                    methodGenerator.Emit(OpCodes.Stfld, info);
                    methodGenerator.Emit(OpCodes.Ldloc, generatedMethod.Locals.Where(local => local.Entry == action.Method).First().Index);

                    counter++;
                }

                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Stfld, action.DynamicType);
            }
        }
    }
}
