using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Siege.Provisions.Mapping.PropertyMappings;

namespace Siege.Provisions.Mapping
{
    public class DomainMapping<TClass> : DomainMapping
    {
        public DomainMapping() : base(typeof(TClass))
        {
        }

        public DomainMapping<TClass> MapID<TType>(Expression<Func<TClass, TType>> expression)
        {
            this.subMappings.Add(new IdMapping<TClass, TType>(expression));
            return this;
        }

        public DomainMapping<TClass> MapID<TType>(Expression<Func<TClass, TType>> expression, string columnName)
        {
            this.subMappings.Add(new IdMapping<TClass, TType>(expression, columnName));
            return this;
        }

        public DomainMapping<TClass> ToTable(string tableName)
        {
            this.table = new Table { Name = tableName };
            return this;
        }

        public DomainMapping<TClass> MapProperty<TType>(Expression<Func<TClass, TType>> expression)
        {
            this.subMappings.Add(new PropertyMapping<TClass, TType>(expression));
            return this;
        }

        public DomainMapping<TClass> MapProperty<TType>(Expression<Func<TClass, TType>> expression, string columnName)
        {
            this.subMappings.Add(new PropertyMapping<TClass, TType>(expression, columnName));
            return this;
        }

        public DomainMapping<TClass> MapList<TType>(Expression<Func<TClass, TType>> expression)
        {
            this.subMappings.Add(new ListMapping<TClass, TType>(expression));
            return this;
        }

        public DomainMapping<TClass> MapComponent<TComponent>(Expression<Func<TClass, TComponent>> expression, Action<ComponentMapping<TClass, TComponent>> mapping)
        {
            var component = new ComponentMapping<TClass, TComponent>(expression);
            mapping(component);
            
            this.subMappings.Add(component);
            return this;
        }

    }

    public class DomainMapping : IDomainMapping
    {
        private readonly Type type;
        protected Table table = new Table();
        protected readonly List<IElementMapping> subMappings = new List<IElementMapping>();
        public Table Table { get { return table; } }

        public DomainMapping(Type type)
        {
            this.type = type;
        }

        public List<IElementMapping> SubMappings
        {
            get { return this.subMappings; }
        }
        
        public DomainMapping MapProperty(PropertyInfo property)
        {
            this.subMappings.Add(new PropertyMapping(property));
            return this;
        }

        public DomainMapping MapComponent(PropertyInfo propertyInfo, Action<ComponentMapping> componentMapping)
        {
            var component = new ComponentMapping(propertyInfo);
            componentMapping(component);
            this.subMappings.Add(component);

            return this;
        }

        public DomainMapping MapID(PropertyInfo property)
        {
            var id = new IdMapping(property);
            this.subMappings.Add(id);

            return this;
        }

        public void Map(Action<DomainMapping> mapping)
        {
            mapping(this);
        }

        public DomainMapping MapForeignRelationship(PropertyInfo property, Type type)
        {
            var foreignRelationshipMapping = new ForeignRelationshipMapping(property, type);
            this.subMappings.Add(foreignRelationshipMapping);

            return this;
        }
    }
}