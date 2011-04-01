using System;
using System.Linq;
using Siege.Provisions.Mapping.Conventions.Identifiers;

namespace Siege.Provisions.Tests.MappingTests
{
    public class ComponentIdentifier : IIdentifier<Type>
    {
        public bool Matches(Type item)
        {
            return item.IsClass && item.GetProperties().Where(p => p.Name == "ID").Count() == 0;
        }
    }
}