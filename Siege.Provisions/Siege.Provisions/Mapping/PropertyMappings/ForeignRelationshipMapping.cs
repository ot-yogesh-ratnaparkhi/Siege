using System;
using System.Reflection;

namespace Siege.Provisions.Mapping.PropertyMappings
{
    public class ForeignRelationshipMapping : ElementMapping
    {
        private readonly Type type;

        public ForeignRelationshipMapping(PropertyInfo property, Type type) : base(property)
        {
            this.type = type;
        }
    }
}