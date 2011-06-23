using System;
using System.Collections.Generic;
using System.Reflection;
using Siege.Requisitions.Dynamic.Ruby;

namespace Siege.Requisitions.Dynamic
{
    public class Install
    {
        public static Action<IServiceLocator> From(string fileName, List<Assembly> assemblies)
        {
            return new RubyInstaller(fileName, assemblies).Build();
        }
    }
}