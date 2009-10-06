using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class ReturnAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        protected readonly MethodInfo method;
        private readonly GeneratedMethod generatedMethod;

        public ReturnAction(MethodBuilderBundle bundle, MethodInfo method, GeneratedMethod generatedMethod)
        {
            this.bundle = bundle;
            this.method = method;
            this.generatedMethod = generatedMethod;
        }

        public virtual void Execute()
        {
            var methodGenerator = this.bundle.MethodBuilder.GetILGenerator();

            if (method.ReturnType != typeof(void))
            {
                methodGenerator.Emit(OpCodes.Ldloc_0);

                if (generatedMethod.LocalCount > 1)
                {
                    methodGenerator.Emit(OpCodes.Stloc_0);
                    methodGenerator.Emit(OpCodes.Ldloc_0);
                }
            }

            methodGenerator.Emit(OpCodes.Ret);
        }
    }
}
