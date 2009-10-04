using System;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class InstantiationAction : ITypeGenerationAction
    {
        private readonly MethodBuilder builder;
        private readonly Type type;
        private readonly Type[] constructorArguments;

        public InstantiationAction(MethodBuilder builder, Type type, Type[] constructorArguments)
        {
            this.builder = builder;
            this.type = type;
            this.constructorArguments = constructorArguments;
        }

        public void Execute()
        {
            this.builder.GetILGenerator().Emit(OpCodes.Newobj, type.GetConstructor(constructorArguments));
        }
    }
}
