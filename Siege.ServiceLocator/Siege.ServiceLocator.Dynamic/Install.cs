using System;
using System.Collections.Generic;
using System.Reflection;
using Siege.ServiceLocator.Dynamic.Ruby;

namespace Siege.ServiceLocator.Dynamic
{
    public class Install
    {
        public static Action<IServiceLocator> From(string fileName, List<Assembly> assemblies)
        {
            return new RubyInstaller(fileName, assemblies).Build();
        }
    }
}