using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    internal class FieldLoadAction : ITypeGenerationAction
    {
        private readonly GeneratedMethod method;
        private readonly Func<FieldInfo> field;
    	private readonly GeneratedField parent;

    	public FieldLoadAction(GeneratedMethod method, Func<FieldInfo> field)
        {
            this.method = method;
            this.field = field;
        }

		public FieldLoadAction(GeneratedMethod method, Func<FieldInfo> field, GeneratedField parent)
		{
			this.method = method;
			this.field = field;
			this.parent = parent;
		}

        public void Execute()
        {
            var methodGenerator = method.MethodBuilder().MethodBuilder.GetILGenerator();

			methodGenerator.Emit(OpCodes.Ldarg_0);

			if (this.parent != null)
			{
				methodGenerator.Emit(OpCodes.Ldfld, parent.Field());
			}

			methodGenerator.Emit(OpCodes.Ldfld, field());
        }
    }
}
