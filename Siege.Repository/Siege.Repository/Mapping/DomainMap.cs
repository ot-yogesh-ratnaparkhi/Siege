using System;

namespace Siege.Repository.Mapping
{
    public class DomainMap
    {
        private readonly DomainMapper mapper = new DomainMapper();

        public Mappings Mappings { get { return mapper.Mappings; } }

        public void Create(Action<DomainMapper> map)
        {
            map(this.mapper);

            this.mapper.Build();
        }

        public void Override<TType>(Action<DomainMapping<TType>> mappingOverrides) where TType : class
        {
            mapper.Override(mappingOverrides);
        }
    }
}