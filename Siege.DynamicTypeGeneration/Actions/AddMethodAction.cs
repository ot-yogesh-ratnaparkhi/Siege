using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class AddMethodAction : ITypeGenerationAction
    {
        private readonly Type returnType;
        private MethodBuilderBundle bundle;

        public AddMethodAction(BuilderBundle bundle, string methodName, Type returnType, Type[] parameterTypes, bool isOverride)
        {
            this.returnType = returnType;
            var methodAttributes = MethodAttributes.Public;
            if (isOverride) methodAttributes |= MethodAttributes.Virtual; 

            this.bundle = new MethodBuilderBundle(bundle)
                              {
                                  MethodBuilder = bundle.TypeBuilder.DefineMethod(
                                      methodName,
                                      methodAttributes,
                                      returnType, parameterTypes)
                              };
        }

        public MethodBuilder MethodBuilder { get { return bundle.MethodBuilder; } }

        public void Execute()
        {
        }
    }
}