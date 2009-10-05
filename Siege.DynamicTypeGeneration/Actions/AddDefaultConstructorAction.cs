using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class AddDefaultConstructorAction : ITypeGenerationAction
    {
        private readonly TypeBuilder builder;
        private ConstructorInfo constructor;
        private ConstructorBuilder constructorBuilder;
        public ConstructorInfo Constructor { get { return this.constructor; } }

        public AddDefaultConstructorAction(BuilderBundle bundle)
        {
            this.builder = bundle.TypeBuilder;
            constructorBuilder = builder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[0]);

            this.constructor = typeof(object).GetConstructor(new Type[0]);
        }

        public void Execute()
        {
            ILGenerator il = constructorBuilder.GetILGenerator();


            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, constructor);
            il.Emit(OpCodes.Ret);
        }
    }
}