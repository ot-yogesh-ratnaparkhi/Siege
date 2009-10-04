using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class FieldAssignmentAction : ITypeGenerationAction
    {
        private readonly MethodBuilder builder;
        private readonly FieldInfo source;
        private readonly IList<ITypeGenerationAction> actions;
        private FieldInfo target;

        public FieldAssignmentAction(MethodBuilder builder, FieldInfo source)
        {
            this.builder = builder;
            this.source = source;
            this.actions = actions;
        }

        public void Execute()
        {
            if(target != null)
            {
                var methodBuilder = builder.GetILGenerator();

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
