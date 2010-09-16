using System;
using System.Collections.Generic;

namespace Siege.Provisions.Mapping
{
    public class DomainMapper
    {
        private readonly List<IDomainMapping> mappings = new List<IDomainMapping>();

        public List<IDomainMapping> Mappings
        {
            get { return mappings; }
        }

        public void Add<TClass>(Action<DomainMapping<TClass>> mapping)
        {
            var map = new DomainMapping<TClass>();
            mapping(map);
            this.Mappings.Add(map);
        }
    }
}