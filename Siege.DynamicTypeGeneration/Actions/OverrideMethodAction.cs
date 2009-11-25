using System.Reflection;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class OverrideMethodAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        private readonly MethodInfo method;

        public OverrideMethodAction(MethodBuilderBundle bundle, MethodInfo method)
        {
            this.bundle = bundle;
            this.method = method;
        }

        public void Execute()
        {
            this.bundle.TypeBuilder.DefineMethodOverride(this.bundle.MethodBuilder, method);
        }
    }
}