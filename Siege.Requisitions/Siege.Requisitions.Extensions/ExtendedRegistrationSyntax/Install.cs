using System;
using System.Collections.Generic;
using System.Reflection;

namespace Siege.Requisitions.Extensions.ExtendedRegistrationSyntax
{
    public class Install
    {
        public static Action<IServiceLocator> From(string fileName, List<Assembly> assemblies)
        {
            return new RubyInstaller.RubyInstaller(fileName, assemblies).Build();
        }
    }
}