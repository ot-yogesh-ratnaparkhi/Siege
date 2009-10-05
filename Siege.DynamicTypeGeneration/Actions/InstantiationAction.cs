using System;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class InstantiationAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        private readonly Type type;
        private readonly Type[] constructorArguments;

        public InstantiationAction(MethodBuilderBundle bundle, Type type, Type[] constructorArguments)
        {
            this.bundle = bundle;
            this.type = type;
            this.constructorArguments = constructorArguments;
        }

        public void Execute()
        {
            this.bundle.MethodBuilder.GetILGenerator().Emit(OpCodes.Newobj, type.GetConstructor(constructorArguments));
        }
    }
}
