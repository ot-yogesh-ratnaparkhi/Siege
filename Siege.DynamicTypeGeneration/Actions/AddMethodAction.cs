using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class AddMethodAction : ITypeGenerationAction
    {
        private readonly TypeBuilder typeBuilder;
        private readonly Type returnType;

        public AddMethodAction(TypeBuilder typeBuilder, string methodName, Type returnType, Type[] parameterTypes)
        {
            this.typeBuilder = typeBuilder;
            this.returnType = returnType;

            MethodBuilder = this.typeBuilder.DefineMethod(
                methodName,
                MethodAttributes.Public | MethodAttributes.Virtual,
                returnType, parameterTypes);
        }

        public MethodBuilder MethodBuilder { get; set; }

        public void Execute()
        {
            var methodGenerator = MethodBuilder.GetILGenerator();

            if (returnType != typeof(void)) methodGenerator.DeclareLocal(returnType);
            methodGenerator.Emit(OpCodes.Nop);
        }
    }
}