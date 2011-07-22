using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Scripting.Hosting;
using Siege.ServiceLocator.Extensions.Conventions;
using Siege.ServiceLocator.Registrations;

namespace Siege.ServiceLocator.Dynamic.Ruby
{
    public class RubyInstaller
    {
        private static readonly object @lock = new object();
        private ScriptEngine engine;
        private ScriptScope scope;
        private readonly string fileName;
        private readonly ScriptSource source;

        public RubyInstaller(string fileName, List<Assembly> assemblies)
        {
            this.fileName = fileName;
            assemblies.AddRange(new[] { typeof(IServiceLocator).Assembly, typeof(IConvention).Assembly });
            
            if(engine == null)
            {
               lock(@lock)
               {
                   if(engine == null)
                   {
                       engine = IronRuby.Ruby.CreateEngine();
                       scope = engine.CreateScope();

                       assemblies.ForEach(a => engine.Runtime.LoadAssembly(a));
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.Installer.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.SiegeDSL.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RubyRegistration.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RegistrationHandlers.RegistrationHandlerFactory.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RegistrationHandlers.DefaultRegistrationHandler.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RegistrationHandlers.DefaultInstanceRegistrationHandler.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RegistrationHandlers.ConditionalRegistrationHandler.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RegistrationHandlers.ConditionalInstanceRegistrationHandler.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RegistrationHandlers.NamedRegistrationHandler.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RegistrationHandlers.NamedInstanceRegistrationHandler.rb");
                   }
               }
            }

            this.source = engine.CreateScriptSourceFromFile(this.fileName);
        }

        public Action<IServiceLocator> Build()
        {
            return locator =>
            {
                engine.Execute(source.GetCode(), scope);
                var @class = engine.Runtime.Globals.GetVariable("Installer");
 	
                var installer = engine.Operations.CreateInstance(@class);
                var registrations = installer.Install();

                var list = new List<IRegistration>();

                foreach(var registration in registrations)
                {
                    if (registration == null) continue;
                    list.Add(registration);
                }

                locator.Register(list);
            };
        }

        private void LoadResource(string name)
        {
            using (var reader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream(name)))
            {
                engine.Execute(engine.CreateScriptSourceFromString(reader.ReadToEnd()).GetCode(), scope);
            }
        }
    }
}