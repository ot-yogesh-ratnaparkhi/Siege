using System;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class AddLocalAction : ITypeGenerationAction
    {
        private readonly MethodBuilder builder;
        private readonly Type localType;

        public AddLocalAction(MethodBuilder builder, Type localType)
        {
            this.builder = builder;
            this.localType = localType;
        }

        public void Execute()
        {
            var methodGenerator = builder.GetILGenerator();

            methodGenerator.DeclareLocal(localType);
            methodGenerator.Emit(OpCodes.Stloc_0);
        }
    }
}
