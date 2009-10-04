using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class AddFieldAction : ITypeGenerationAction
    {
        private readonly FieldInfo field;
        private readonly FieldBuilder fieldBuilder;
        public FieldInfo Field { get { return fieldBuilder; } }
        public FieldInfo Source { get { return field; } }

        public AddFieldAction(TypeBuilder builder, FieldInfo field)
        {
            this.field = field;
            fieldBuilder = builder.DefineField(field.Name, field.FieldType, FieldAttributes.Public);
        }

        public void Execute()
        {

        }
    }
}
