using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class SetFuncTargetAction : ITypeGenerationAction
    {
        private readonly MethodBuilder builder;
        private readonly MethodInfo method;
        private IList<ITypeGenerationAction> actions;
        private readonly GeneratedMethod generatedMethod;

        public SetFuncTargetAction(MethodBuilder builder, MethodInfo method, IList<ITypeGenerationAction> actions, GeneratedMethod generatedMethod)
        {
            this.builder = builder;
            this.method = method;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
        }

        public void Execute()
        {
            var methodGenerator = builder.GetILGenerator();

            methodGenerator.Emit(OpCodes.Ldarg_0);
            methodGenerator.Emit(OpCodes.Ldftn, method);
        }

        public CallAction Call(MethodInfo method)
        {
            var action = new CallAction(builder, method, actions, generatedMethod);
            this.actions.Add(action);

            return action;
        }
    }
}
