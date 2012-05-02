using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Scripting.Hosting;
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

        public RubyInstaller(string fileName)
        {
            this.fileName = fileName;
            var binFiles =  Assembly.GetEntryAssembly() != null ? Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).ToList() : new List<string>();
            
            var assemblies = new List<Assembly>();
            var appDomain = new List<Assembly>();
            appDomain.AddRange(AppDomain.CurrentDomain.GetAssemblies());
            
            foreach(var file in binFiles)
            {
                foreach (var assembly in appDomain)
                {
                    try
                    {
                        if (assembly.Location == file || (!file.EndsWith(".dll") && !file.EndsWith(".exe"))) continue;

                        assemblies.Add(Assembly.LoadFile(file));
                    }
                    catch (Exception)
                    {
                        //don't load the assembly
                    }
                }
            }

            assemblies.AddRange(appDomain);

            if(engine == null)
            {
               lock(@lock)
               {
                   if(engine == null)
                   {
                       engine = IronRuby.Ruby.CreateEngine();
                       
                       var paths = engine.GetSearchPaths().ToList();
                       paths.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                       engine.SetSearchPaths(paths);
                       
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
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RegistrationHandlers.ConventionRegistrationHandler.rb");
                       LoadResource("Siege.ServiceLocator.Dynamic.Scripts.RegistrationHandlers.ConventionInstanceRegistrationHandler.rb");
                   }
               }
            }

            this.source = engine.CreateScriptSourceFromFile(this.fileName);
        }

        public Action<IServiceLocator> Build()
        {
            return locator =>
            {
                engine.Execute("include Siege::ServiceLocator::Registrations", scope);
                engine.Execute("include Siege::ServiceLocator::RegistrationPolicies", scope);
                engine.Execute("include Siege::ServiceLocator::ResolutionRules", scope);
                engine.Execute("include Siege::ServiceLocator::Registrations::Conventions", scope);

                
                engine.Execute(source.GetCode(), scope);
                var @class = engine.Runtime.Globals.GetVariable("Installer");
 	
                var installer = engine.Operations.CreateInstance(@class);
                var registrations = installer.Install();

                var list = new List<IRegistration>();

                foreach(var registration in registrations)
                {
                    if (registration == null) continue;
                    if (registration is Action<IServiceLocator>)
                    {
                        locator.Register(registration);
                        continue;
                    }
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