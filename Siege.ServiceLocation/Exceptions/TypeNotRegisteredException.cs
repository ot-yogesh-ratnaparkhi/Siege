using System;

namespace Siege.ServiceLocation.Exceptions
{
    public class TypeNotRegisteredException : ApplicationException
    {
        public TypeNotRegisteredException(Type type) : base("Type not registered: " + type)
        {
            
        }
    }
}
