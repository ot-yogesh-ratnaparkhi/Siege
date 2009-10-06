using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CreateFuncAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        private IList<ITypeGenerationAction> actions;
        private readonly GeneratedMethod generatedMethod;
        private readonly Type[] parameters;
        private ConstructorInfo funcConstructor;
        private static Hashtable generatedTypes = new Hashtable();

        public CreateFuncAction(MethodBuilderBundle bundle, Type returnType, IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
            this.parameters = parameters;

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
            private readonly MethodInfo method;
            private readonly CreateFuncEncapsulationAction action;

            public LoadNestedTypeAction(MethodBuilderBundle bundle, CreateFuncEncapsulationAction action, GeneratedMethod generatedMethod)
            {
                this.generatedMethod = generatedMethod;
                this.bundle = bundle;
                this.method = method;
                this.action = action;
            }

            public void Execute()
            {
                var methodGenerator = bundle.MethodBuilder.GetILGenerator();

				methodGenerator.Emit(OpCodes.Newobj, action.Constructor);
                methodGenerator.Emit(OpCodes.Stloc, (int)generatedMethod.Locals[action.Method]);
                methodGenerator.Emit(OpCodes.Ldloc, (int)generatedMethod.Locals[action.Method]);

                int counter = 1;
                foreach(FieldInfo info in action.PublicFields)
                {
                    methodGenerator.Emit(OpCodes.Ldarg, counter);
                    methodGenerator.Emit(OpCodes.Stfld, info);
                    methodGenerator.Emit(OpCodes.Ldloc, (int)generatedMethod.Locals[action.Method]);

                    counter++;
                }

                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Stfld, action.DynamicType);
            }
        }
    }
}
