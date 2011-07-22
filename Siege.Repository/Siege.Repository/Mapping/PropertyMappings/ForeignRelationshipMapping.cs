using System;
using System.Reflection;
using Siege.Repository.Mapping.Conventions.Formatters;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class ForeignRelationshipMapping : PropertyMapping
    {
        private readonly Type type;
        private readonly Type parentType;

        public ForeignRelationshipMapping(PropertyInfo property, Type type, Formatter<PropertyInfo> keyFormatter) : base(property)
        {
            this.type = type;
            this.parentType = parentType;
            this.ColumnName = keyFormatter.Format(property);
        }
    }
}