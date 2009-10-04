using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class AddDefaultConstructorAction : ITypeGenerationAction
    {
        private readonly TypeBuilder builder;

        public AddDefaultConstructorAction(TypeBuilder builder)
        {
            this.builder = builder;
        }

        public void Execute()
        {
            ConstructorBuilder constructor = builder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[0]);


            ConstructorInfo conObj = typeof(object).GetConstructor(new Type[0]);

            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Ret);
        }
    }
}