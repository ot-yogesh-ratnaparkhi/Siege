using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Siege.Provisions.Mapping.PropertyMappings;

namespace Siege.Provisions.Mapping
{
    public class DomainMapping<TClass> : IDomainMapping
    {
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
            this.table = tableName;
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

        private string table;
        private readonly List<IElementMapping> subMappings = new List<IElementMapping>();

        public string Table { get { return table; } }

        public List<IElementMapping> SubMappings
        {
            get { return this.subMappings; }
        }
    }
}