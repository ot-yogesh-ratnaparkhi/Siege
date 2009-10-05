using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class CaptureCallResultAction : ITypeGenerationAction
    {
        private readonly MethodBuilder builder;
        private readonly MethodInfo method;
        private readonly GeneratedMethod generatedMethod;

        public CaptureCallResultAction(MethodBuilder builder, MethodInfo method, GeneratedMethod generatedMethod)
        {
            this.builder = builder;
            this.method = method;
            this.generatedMethod = generatedMethod;
        }

        public void Execute()
        {
            var methodGenerator = builder.GetILGenerator();

            methodGenerator.DeclareLocal(method.ReturnType);
            methodGenerator.Emit(OpCodes.Stloc_0);

            generatedMethod.AddLocal();
        }
    }
}