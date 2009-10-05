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
                for (int i = 0; i < generatedMethod.LocalCount; i++)
                {
                    methodGenerator.Emit(OpCodes.Ldloc, i);
                }   
            }

            methodGenerator.Emit(OpCodes.Ret);
        }
    }
}
