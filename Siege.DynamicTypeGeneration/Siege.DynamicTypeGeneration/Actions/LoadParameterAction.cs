using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class LoadParameterAction : ITypeGenerationAction
    {
        private readonly Func<GeneratedMethod> method;
        private readonly int index;

        public LoadParameterAction(Func<GeneratedMethod> method, int index)
        {
            this.method = method;
            this.index = index;
        }

        public void Execute()
        {
            method().MethodBuilder().MethodBuilder.GetILGenerator().Emit(OpCodes.Ldarg, index);
        }
    }
}
