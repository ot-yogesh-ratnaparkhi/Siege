using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CourierApp
{
    [RunInstaller(true)]
    public class ServiceBusInstaller : Installer
    {
        private readonly ServiceProcessInstaller process;
        private readonly ServiceInstaller service;

        public ServiceBusInstaller()
        {
            process = new ServiceProcessInstaller {Account = ServiceAccount.LocalSystem};
            service = new ServiceInstaller {ServiceName = "WCF Service Bus"};
            Installers.Add(process);
            Installers.Add(service);
        }
    }

}