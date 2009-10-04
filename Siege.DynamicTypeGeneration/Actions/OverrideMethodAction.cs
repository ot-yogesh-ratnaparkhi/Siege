using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class OverrideMethodAction : ITypeGenerationAction
    {
        private readonly TypeBuilder builder;
        private readonly MethodBuilder methodBuilder;
        private readonly MethodInfo method;

        public OverrideMethodAction(TypeBuilder builder, MethodBuilder methodBuilder, MethodInfo method)
        {
            this.builder = builder;
            this.methodBuilder = methodBuilder;
            this.method = method;
        }

        public void Execute()
        {
            builder.DefineMethodOverride(methodBuilder, method);
        }
    }
}
