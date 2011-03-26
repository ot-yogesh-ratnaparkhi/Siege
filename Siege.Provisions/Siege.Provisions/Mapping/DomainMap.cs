using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.Provisions.Mapping
{
    public class DomainMap
    {
        private readonly DomainMapper mapper = new DomainMapper();

        public void Create(Action<DomainMapper> map)
        {
            map(this.mapper);

            this.mapper.Build();
        }

        public Mappings Mappings { get { return mapper.Mappings; } }
    }

    public class Mappings
    {
        private readonly Dictionary<Type, IDomainMapping> mappings = new Dictionary<Type, IDomainMapping>();

        public void Add<TType>(IDomainMapping mapping) where TType : class
        {
            Add(typeof (TType), mapping);
        }

        public void Add(Type type, IDomainMapping mapping)
        {
            this.mappings.Add(type, mapping);
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