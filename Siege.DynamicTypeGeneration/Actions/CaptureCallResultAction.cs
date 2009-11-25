using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CaptureCallResultAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;

        public CaptureCallResultAction(MethodBuilderBundle bundle, MethodInfo method, GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
        }

        public void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();

            methodGenerator.Emit(OpCodes.Stloc_0);
        }
    }
}