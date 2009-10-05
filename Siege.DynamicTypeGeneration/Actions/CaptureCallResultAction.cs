using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CaptureCallResultAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        private readonly MethodInfo method;
        private readonly GeneratedMethod generatedMethod;

        public CaptureCallResultAction(MethodBuilderBundle bundle, MethodInfo method, GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
            this.method = method;
            this.generatedMethod = generatedMethod;
        }

        public void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();

            methodGenerator.DeclareLocal(method.ReturnType);
            methodGenerator.Emit(OpCodes.Stloc_0);

            generatedMethod.AddLocal();
        }
    }
}