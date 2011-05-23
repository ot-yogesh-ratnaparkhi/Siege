using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Scripting.Hosting;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Registrations;

namespace Siege.Requisitions.Dynamic.Ruby
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
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.Installer.rb");
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.SiegeDSL.rb");
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.RubyRegistration.rb");
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.RegistrationHandlers.RegistrationHandlerFactory.rb");
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.RegistrationHandlers.DefaultRegistrationHandler.rb");
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.RegistrationHandlers.DefaultInstanceRegistrationHandler.rb");
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.RegistrationHandlers.ConditionalRegistrationHandler.rb");
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.RegistrationHandlers.ConditionalInstanceRegistrationHandler.rb");
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.RegistrationHandlers.NamedRegistrationHandler.rb");
                       LoadResource("Siege.Requisitions.Dynamic.Scripts.RegistrationHandlers.NamedInstanceRegistrationHandler.rb");
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