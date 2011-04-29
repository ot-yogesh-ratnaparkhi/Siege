using System;

namespace Siege.Requisitions.Extensions.ExtendedRegistrationSyntax
{
    public class Install
    {
        public static Action<IServiceLocator> From(string fileName)
        {
            return new PythonInstaller.PythonInstaller(fileName).Build();
        }
    }
}