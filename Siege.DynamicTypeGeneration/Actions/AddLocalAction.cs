using System;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class AddLocalAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        private readonly Type type;

        public AddLocalAction(MethodBuilderBundle bundle, Type type)
        {
            this.bundle = bundle;
            this.type = type;
        }

        public void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();
            methodGenerator.DeclareLocal(type);
        }
    }
}