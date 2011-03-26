using System;
using System.Collections.Generic;
using Siege.Provisions.Mapping.Conventions;

namespace Siege.Provisions.Mapping
{
    public class DomainMapper
    {
        private readonly Mappings mappings = new Mappings();
        private readonly List<IConvention> conventions = new List<IConvention>();

        public Mappings Mappings
        {
            get { return mappings; }
        }

        public void Add<TClass>(Action<DomainMapping<TClass>> mapping) where TClass : class
        {
            var map = new DomainMapping<TClass>();
            mapping(map);
            this.Mappings.Add<TClass>(map);
        }

        public void Add(Type type, Action<DomainMapping> mapping)
        {
            var map = new DomainMapping();
            mapping(map);
            this.Mappings.Add(type, map);
        }

        public IDomainMapping For(Type type)
        {
            return this.mappings.For(type);
        }

        public void Add<TClass>() where TClass : class
        {
            this.Mappings.Add<TClass>(new DomainMapping<TClass>());
        }

        public void UseConvention<TConvention>(Action<TConvention> convention) where TConvention : IConvention, new()
        {
            var instance = new TConvention();

            convention(instance);

            this.conventions.Add(instance);
        }

        public void Build()
        {
            foreach(IConvention convention in this.conventions)
            {
                foreach(Type type in mappings.MappedTypes)
                {
                    convention.Map(type, this);
                }
            }
        }
    }
}