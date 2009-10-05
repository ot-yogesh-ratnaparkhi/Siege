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
            //CreateFuncEncapsulationAction nestedType;

            //if (!generatedTypes.ContainsKey(bundle.TypeBuilder.Name))
            //{
            //    nestedType = new CreateFuncEncapsulationAction(bundle, method, this.actions);

            //    //this.actions.Add(nestedType);
            //    generatedTypes.Add(bundle.TypeBuilder.Name, nestedType);
            //}
            //else
            //{
            //    nestedType = (CreateFuncEncapsulationAction) generatedTypes[bundle.TypeBuilder.Name];
            //}
            
            //this.actions.Add(new LoadNestedTypeAction(bundle, nestedType.LocalBuilder, method, nestedType));
            var action = new SetFuncTargetAction(this.bundle, method, this.actions, generatedMethod);
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
            private readonly MethodBuilderBundle bundle;
            private readonly Type type;
            private readonly MethodInfo method;
            private readonly CreateFuncEncapsulationAction action;

            public LoadNestedTypeAction(MethodBuilderBundle bundle, Type type, MethodInfo method, CreateFuncEncapsulationAction action)
            {
                this.bundle = bundle;
                this.type = type;
                this.method = method;
                this.action = action;
            }

            public void Execute()
            {
                if (method.DeclaringType.GetCustomAttributes(typeof(DynamicTypeAttribute), true).Length > 0) return;

                var methodGenerator = bundle.MethodBuilder.GetILGenerator();
                
                methodGenerator.Emit(OpCodes.Newobj, action.Constructor);
                methodGenerator.Emit(OpCodes.Stloc_0);
                methodGenerator.Emit(OpCodes.Ldloc_0);

                int counter = 1;
                foreach(ParameterInfo parameter in method.GetParameters())
                {
                    FieldInfo info = type.GetField(parameter.Name);
                    methodGenerator.Emit(OpCodes.Ldloc_0);
                    methodGenerator.Emit(OpCodes.Ldarg, counter);
                    //methodGenerator.Emit(OpCodes.Stfld, info);

                    counter++;
                }

                methodGenerator.Emit(OpCodes.Ldarg_0);
                //methodGenerator.Emit(OpCodes.Stfld, type.GetField(bundle.TypeBuilder.Name));
            }
        }
    }
}
