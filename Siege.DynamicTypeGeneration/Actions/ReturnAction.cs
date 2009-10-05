using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class ReturnAction : ITypeGenerationAction
    {
        protected readonly MethodBuilder builder;
        protected readonly MethodInfo method;
        private readonly GeneratedMethod generatedMethod;

        public ReturnAction(MethodBuilder builder, MethodInfo method, GeneratedMethod generatedMethod)
        {
            this.builder = builder;
            this.method = method;
            this.generatedMethod = generatedMethod;
        }

        public virtual void Execute()
        {
            var methodGenerator = builder.GetILGenerator();

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
