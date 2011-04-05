using System;
using System.Collections;
using System.Linq;

namespace Siege.Provisions.Mapping.Conventions.Identifiers
{
    public class GenericListIdentifier : IIdentifier<Type>
    {
        public bool Matches(Type type)
        {
            return type.IsGenericType &&
                   type.GetInterfaces().Where(i => i == typeof (IEnumerable)).Count() > 0;
        }
    }
}