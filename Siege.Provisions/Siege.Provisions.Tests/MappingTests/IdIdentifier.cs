using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Identifiers;

namespace Siege.Provisions.Tests.MappingTests
{
    public class IdIdentifier : IIdentifier<PropertyInfo>
    {
        public bool Matches(PropertyInfo item)
        {
            return item.Name == "ID";
        }
    }
}