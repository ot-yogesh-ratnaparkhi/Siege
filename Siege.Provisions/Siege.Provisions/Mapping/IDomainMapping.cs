using System;
using System.Collections.Generic;

namespace Siege.Provisions.Mapping
{
    public interface IDomainMapping
    {
        Table Table { get; }
        List<IElementMapping> SubMappings { get; }
        void Map(Action<DomainMapping> mapping);
    }
}