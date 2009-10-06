using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class SetFuncTargetAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        private readonly MethodInfo method;
        private IList<ITypeGenerationAction> actions;
        private readonly GeneratedMethod generatedMethod;

        public SetFuncTargetAction(MethodBuilderBundle bundle, MethodInfo method, IList<ITypeGenerationAction> actions,
                                   GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
            this.method = method;
            this.actions = actions;
            this.generatedMethod = generatedMethod;
        }

        public void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();

            if (generatedMethod.Locals.ContainsKey(method))
            {
                methodGenerator.Emit(OpCodes.Ldloc, (int)generatedMethod.Locals[method]);
            }
            else
            {
                methodGenerator.Emit(OpCodes.Ldarg_0);
            }
            methodGenerator.Emit(OpCodes.Ldftn, method);
        }

        public CallAction Call(MethodInfo method)
        {
            var action = new CallAction(bundle, method, actions, generatedMethod);
            this.actions.Add(action);
            return action;
        }
    }
}