using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CreateFuncAction : ITypeGenerationAction
    {
        private readonly MethodBuilder builder;
        private IList<ITypeGenerationAction> actions;
        private ConstructorInfo funcConstructor;

        public CreateFuncAction(MethodBuilder builder, Type returnType, IList<ITypeGenerationAction> actions)
        {
            this.builder = builder;
            this.actions = actions;

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
            var action = new SetFuncTargetAction(this.builder, method, this.actions);
            this.actions.Add(action);
            this.actions.Add(this);

            return action;
        }

        public void Execute()
        {
            var methodGenerator = builder.GetILGenerator();

            methodGenerator.Emit(OpCodes.Newobj, funcConstructor);
        }
    }
}
