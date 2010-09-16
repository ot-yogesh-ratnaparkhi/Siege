using System.Collections.Generic;

namespace Siege.Provisions.Mapping
{
    public interface IDomainMapping
    {
        string Table { get; }
        List<IElementMapping> SubMappings { get; }
    }
}