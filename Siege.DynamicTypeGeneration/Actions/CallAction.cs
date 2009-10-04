using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CallAction : ITypeGenerationAction
    {
        protected readonly MethodBuilder builder;
        protected readonly MethodInfo method;
        protected IList<ITypeGenerationAction> actions;
        protected FieldInfo target;

        public CallAction(MethodBuilder builder, MethodInfo method, IList<ITypeGenerationAction> actions)
        {
            this.builder = builder;
            this.method = method;
            this.actions = actions;
        }

        public void On(FieldInfo field)
        {
            target = field;
        }

        public virtual void Execute()
        {
            var methodGenerator = builder.GetILGenerator();

            if (target != null)
            {
                methodGenerator.Emit(OpCodes.Ldarg_0);

                for (int i = 1; i <= method.GetParameters().Length; i++)
                {
                    methodGenerator.Emit(OpCodes.Ldarg, i);
                }
                
                methodGenerator.Emit(OpCodes.Ldfld, target);
            }

            methodGenerator.Emit(OpCodes.Call, method);
            methodGenerator.Emit(OpCodes.Nop);

            if(method.ReturnType != typeof(void))
            {
                methodGenerator.DeclareLocal(method.ReturnType);
            }
        }
    }
}
