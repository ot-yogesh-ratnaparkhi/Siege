using System;
using System.Collections.Generic;

namespace Siege.Repository.Mapping
{
    public interface IDomainMapping
    {
        Table Table { get; }
        List<IElementMapping> SubMappings { get; }
        void Map(Action<DomainMapping> mapping);
    }
}