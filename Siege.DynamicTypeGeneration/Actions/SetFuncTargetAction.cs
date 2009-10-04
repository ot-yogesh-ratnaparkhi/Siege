using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class SetFuncTargetAction : ITypeGenerationAction
    {
        private readonly MethodBuilder builder;
        private readonly MethodInfo method;
        private readonly IList<ITypeGenerationAction> actions;

        public SetFuncTargetAction(MethodBuilder builder, MethodInfo method, IList<ITypeGenerationAction> actions)
        {
            this.builder = builder;
            this.method = method;
            this.actions = actions;
        }

        public void Execute()
        {
            var methodGenerator = builder.GetILGenerator();

            methodGenerator.Emit(OpCodes.Ldarg_0);
            methodGenerator.Emit(OpCodes.Ldftn, method);
        }

        public CallAction Call(MethodInfo method)
        {
            var action = new CallAction(builder, method, actions);
            this.actions.Add(action);

            return action;
        }
    }
}
