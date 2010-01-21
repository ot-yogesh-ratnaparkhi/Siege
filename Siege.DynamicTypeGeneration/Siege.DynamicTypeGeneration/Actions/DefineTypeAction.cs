using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    internal class DefineTypeAction : ITypeGenerationAction
    {
        protected readonly BuilderBundle bundle;
        protected readonly Func<string> typeName;
        protected readonly Func<Type> baseType;
        internal TypeBuilder TypeBuilder { get; private set; }

        public DefineTypeAction(BuilderBundle bundle, Func<string> typeName, Func<Type> baseType)
        {
            this.bundle = bundle;
            this.typeName = typeName;
            this.baseType = baseType;
        }

        public void Execute()
        {
            TypeBuilder = DefineType();
        }

        protected virtual TypeBuilder DefineType()
        {
            return bundle.TypeBuilder = bundle.ModuleBuilder.DefineType(typeName(),
                                                   TypeAttributes.Public |
                                                   TypeAttributes.Class |
                                                   TypeAttributes.AutoClass |
                                                   TypeAttributes.AnsiClass |
                                                   TypeAttributes.BeforeFieldInit |
                                                   TypeAttributes.AutoLayout,
                                                   baseType());
        }
    }
}
