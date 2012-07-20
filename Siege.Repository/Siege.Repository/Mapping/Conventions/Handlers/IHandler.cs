﻿using System;
using System.Reflection;

namespace Siege.Repository.Mapping.Conventions.Handlers
{
    public interface IHandler
    {
        bool CanHandle(PropertyInfo property);
        void Handle(PropertyInfo property, Type type, DomainMapping mapper);
    }
}