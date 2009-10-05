using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class FieldAssignmentAction : ITypeGenerationAction
    {
        private readonly MethodBuilderBundle bundle;
        private readonly FieldInfo source;
        private FieldInfo target;

        public FieldAssignmentAction(MethodBuilderBundle bundle, FieldInfo source)
        {
            this.bundle = bundle;
            this.source = source;
        }

        public void Execute()
        {
            if(target != null)
            {
                var methodBuilder = this.bundle.MethodBuilder.GetILGenerator();

                methodBuilder.Emit(OpCodes.Ldarg_0);
                methodBuilder.Emit(OpCodes.Ldarg_0);
                methodBuilder.Emit(OpCodes.Ldfld, source);
                methodBuilder.Emit(OpCodes.Stfld, target);
            }
        }

        public void To(FieldInfo field)
        {
            target = field;
        }
    }
}
