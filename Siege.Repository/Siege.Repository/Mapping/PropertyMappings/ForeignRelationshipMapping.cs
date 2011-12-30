using System.Reflection;
using Siege.Repository.Mapping.Conventions.Formatters;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class ForeignRelationshipMapping : PropertyMapping
    {
        private readonly PropertyInfo id;

        public ForeignRelationshipMapping(PropertyInfo property, PropertyInfo id, Formatter<PropertyInfo> keyFormatter) : base(property, keyFormatter.Format(property))
        {
            this.id = id;
        }

        public override object GetValue(object item)
        {
            var instance = base.GetValue(item);

            return id.GetValue(instance, new object[0]);
        }
    }
}