using System;
using Siege.ServiceLocator.Dynamic.Ruby;

namespace Siege.ServiceLocator.Dynamic
{
    public class Install
    {
        public static Action<IServiceLocator> From(string fileName)
        {
            return new RubyInstaller(fileName).Build();
        }
    }
}