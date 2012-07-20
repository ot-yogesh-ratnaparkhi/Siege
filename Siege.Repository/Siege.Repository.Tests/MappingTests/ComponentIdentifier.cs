using System;
using System.Linq;
using Siege.Repository.Mapping.Conventions.Identifiers;

namespace Siege.Repository.Tests.MappingTests
{
    public class ComponentIdentifier : IIdentifier<Type>
    {
        public bool Matches(Type item)
        {
            return item.IsClass && item.GetProperties().Where(p => p.Name == "ID").Count() == 0;
        }
    }
}