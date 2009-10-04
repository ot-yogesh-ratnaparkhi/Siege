using System.Reflection;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class GeneratedField
    {
        private readonly AddFieldAction action;

        public GeneratedField(AddFieldAction action)
        {
            this.action = action;
        }

        public FieldInfo Field { get { return action.Field; } }
        public FieldInfo Source { get { return action.Source; } }
    }
}
