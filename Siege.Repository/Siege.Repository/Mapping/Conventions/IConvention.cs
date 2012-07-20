using System;

namespace Siege.Repository.Mapping.Conventions
{
    public interface IConvention
    {
        void Map(Type type, DomainMapping mapper);
    }
}