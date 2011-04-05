using System;
using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Formatters;

namespace Siege.Provisions.Mapping.PropertyMappings
{
    public class ForeignRelationshipMapping : PropertyMapping
    {
        private readonly Type type;

        public ForeignRelationshipMapping(PropertyInfo property, Type type, Formatter<PropertyInfo> keyFormatter) : base(property)
        {
            this.type = type;

            this.ColumnName = keyFormatter.Format(property);
        }
    }
}