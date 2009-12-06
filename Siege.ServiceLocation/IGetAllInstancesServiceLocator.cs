using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IGetAllInstancesServiceLocator
    {
        IEnumerable<object> GetAllInstances(Type serviceType);
        IEnumerable<TService> GetAllInstances<TService>();
    }
}