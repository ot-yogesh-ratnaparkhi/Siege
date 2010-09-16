using System;
using System.Collections.Generic;

namespace Siege.Provisions.Mapping
{
    public class DomainMap
    {
        private readonly DomainMapper mapper = new DomainMapper();

        public void Create(Action<DomainMapper> map)
        {
            map(this.mapper);
        }

        public List<IDomainMapping> Mappings { get { return mapper.Mappings; } }
    }
}