using System;
using System.Collections.Generic;
using System.Reflection;
using IronRuby;
using Microsoft.Scripting.Hosting;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Registrations;

namespace Siege.Requisitions.Extensions.RubyInstaller
{
    public class RubyInstaller : IConvention
    {
        private readonly string fileName;
        private readonly ScriptEngine engine;
        private readonly ScriptSource source;

        public RubyInstaller(string fileName)
        {
            this.fileName = fileName;
            this.engine = Ruby.CreateEngine();
            this.source = engine.CreateScriptSourceFromFile(this.fileName);
        }

        public Action<IServiceLocator> Build()
        {
            return locator =>
            {
                var expression = this.source.GetCode();
                var local = engine.CreateScriptSourceFromString(expression);

                engine.Execute(local.GetCode());
                engine.Runtime.LoadAssembly(this.GetType().Assembly);
                engine.Runtime.LoadAssembly(Assembly.GetExecutingAssembly());
                var @class = engine.Runtime.Globals.GetVariable("ClrInstaller");
                var installer = engine.Operations.CreateInstance(@class);
               
                dynamic instance = installer.get_Current();
                var instances = instance.Build();

                var list = new List<IRegistration>();
                
                foreach(IRegistration item in instances)
                {
                    if (item == null) continue;

                    list.Add(item);
                }

                locator.Register(list);
            };
        }
    }
}