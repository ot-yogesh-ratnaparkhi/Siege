using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class ReturnAction : ITypeGenerationAction
    {
        protected readonly MethodBuilder builder;
        protected readonly MethodInfo method;

        public ReturnAction(MethodBuilder builder, MethodInfo method)
        {
            this.builder = builder;
            this.method = method;
        }

        public virtual void Execute()
        {
            var methodGenerator = builder.GetILGenerator();

            if (method.ReturnType != typeof(void))
            {
                methodGenerator.Emit(OpCodes.Stloc_0);
                methodGenerator.Emit(OpCodes.Ldloc_0);
                methodGenerator.Emit(OpCodes.Stloc_1);
                methodGenerator.Emit(OpCodes.Ldloc_1);
            }

            methodGenerator.Emit(OpCodes.Nop);
            methodGenerator.Emit(OpCodes.Ret);
        }
    }
}
