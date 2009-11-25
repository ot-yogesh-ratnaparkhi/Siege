using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration.Actions
{
    public class AddFieldAction : ITypeGenerationAction
    {
        private FieldInfo field;
        private FieldBuilder fieldBuilder;

        public FieldInfo Field
        {
            get { return fieldBuilder; }
        }

        public FieldInfo Source
        {
            get { return field; }
        }

        public AddFieldAction(BuilderBundle bundle, FieldInfo field)
            : this(bundle, field.Name, field.FieldType)
        {
            this.field = field;
        }

        public AddFieldAction(BuilderBundle bundle, string fieldName, Type fieldType)
        {
            this.field = fieldBuilder = bundle.TypeBuilder.DefineField(fieldName, fieldType, FieldAttributes.Public);
        }

        public void Execute()
        {
        }
    }
}