using System;

namespace Siege.Provisions.Mapping.Conventions
{
    public interface IConvention
    {
        void Map(Type type, DomainMapper mapper);
    }
}