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
        private readonly string fileName;
        private readonly List<Assembly> assemblies;
        private readonly ScriptEngine engine;
        private readonly ScriptSource source;

        public RubyInstaller(string fileName, List<Assembly> assemblies)
        {
            this.fileName = fileName;
            this.assemblies = assemblies;
            this.assemblies.AddRange(new[] { typeof(IServiceLocator).Assembly, typeof(IConvention).Assembly });
            this.engine = IronRuby.Ruby.CreateEngine();
            this.source = engine.CreateScriptSourceFromFile(this.fileName);
        }

        public Action<IServiceLocator> Build()
        {
            return locator =>
            {
                var scope = engine.CreateScope();

                assemblies.ForEach(a => engine.Runtime.LoadAssembly(a));

                using (var reader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Siege.Requisitions.Dynamic.Scripts.Installer.rb")))
                {
                    engine.Execute(engine.CreateScriptSourceFromString(reader.ReadToEnd()).GetCode(), scope);
                }

                using (var reader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Siege.Requisitions.Dynamic.Scripts.SiegeDSL.rb")))
                {
                    engine.Execute(engine.CreateScriptSourceFromString(reader.ReadToEnd()).GetCode(), scope);
                }

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
    }
}