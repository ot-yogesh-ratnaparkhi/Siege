using System;
using System.Reflection;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class ListMapping : ElementMapping
    {
        private readonly Type type;
        private readonly ReverseForeignRelationshipMapping foreignRelationshipMapping;

        public ListMapping(PropertyInfo property, Type type, ReverseForeignRelationshipMapping foreignRelationshipMapping) : base(property)
        {
            this.type = type;
            this.foreignRelationshipMapping = foreignRelationshipMapping;
        }

        public ReverseForeignRelationshipMapping ForeignRelationshipMapping
        {
            get { return foreignRelationshipMapping; }
        }

        public Type Type
        {
            get { return type; }
        }
    }
}