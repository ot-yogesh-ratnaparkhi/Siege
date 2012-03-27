using System;
using System.Collections.Generic;

namespace Siege.Security.Providers
{
    public interface IApplicationProvider : IProvider<Application>
    {
        IList<Application> GetAllApplications();
    }
}