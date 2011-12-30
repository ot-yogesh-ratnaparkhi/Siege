using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.Repository.Mapping
{
    public class Mappings
    {
        private readonly Dictionary<Type, IDomainMapping> mappings = new Dictionary<Type, IDomainMapping>();

        public void Add<TType>(IDomainMapping mapping) where TType : class
        {
            Add(typeof (TType), mapping);
        }

        public void Add(Type type, IDomainMapping mapping)
        {
            if(!mappings.ContainsKey(type))
            {
                this.mappings.Add(type, mapping);
            }
            else
            {
                this.mappings[type] = mapping;
            }
        }

        public bool Contains(Type type)
        {
            return this.mappings.ContainsKey(type);
        }

        public int Count { get { return mappings.Count; } }

        public IDomainMapping For<TClass>() where TClass : class
        {
            return For(typeof (TClass));
        }

        public IDomainMapping For(Type type)
        {
            return this.mappings[type];
        }

        public IEnumerable MappedTypes { get { return this.mappings.Keys;  } }
    }
}