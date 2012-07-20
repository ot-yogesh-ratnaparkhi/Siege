﻿using System.Collections.Generic;
using Siege.Security.Entities;

namespace Siege.Security.Providers
{
    public interface ISecurityProvider<T> : IProvider<T> where T : ConsumerBasedSecurityEntity
    {
        IList<T> GetForConsumer(Consumer consumer, bool includeHiddenPermissions);
        IList<T> GetForApplicationAndConsumer(Application application, Consumer consumer, bool includeHiddenPermissions);
    }
}