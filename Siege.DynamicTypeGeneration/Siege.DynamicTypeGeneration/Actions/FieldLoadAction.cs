using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    internal class FieldLoadAction : ITypeGenerationAction
    {
        private readonly GeneratedMethod method;
        private readonly Func<FieldInfo> field;

        public FieldLoadAction(GeneratedMethod method, Func<FieldInfo> field)
        {
            this.method = method;
            this.field = field;
        }

        public void Execute()
        {
            var methodGenerator = method.MethodBuilder().MethodBuilder.GetILGenerator();

            methodGenerator.Emit(OpCodes.Ldarg_0);
            methodGenerator.Emit(OpCodes.Ldfld, field());
        }
    }
}
