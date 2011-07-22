using System.Reflection;
using Siege.Repository.Mapping.Conventions.Identifiers;

namespace Siege.Repository.Tests.MappingTests
{
    public class IdIdentifier : IIdentifier<PropertyInfo>
    {
        public bool Matches(PropertyInfo item)
        {
            return item.Name == "ID";
        }
    }
}