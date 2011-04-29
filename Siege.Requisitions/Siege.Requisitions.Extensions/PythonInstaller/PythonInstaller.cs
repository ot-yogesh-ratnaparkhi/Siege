using System;
using System.Reflection;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Siege.Requisitions.Extensions.Conventions;

namespace Siege.Requisitions.Extensions.PythonInstaller
{
    public class PythonInstaller : IConvention
    {
        private readonly string fileName;
        private readonly ScriptEngine engine;
        private readonly ScriptSource source;

        public PythonInstaller(string fileName)
        {
            this.fileName = fileName;
            this.engine = Python.CreateEngine();
            this.source = engine.CreateScriptSourceFromFile(this.fileName);
        }

        public Action<IServiceLocator> Build()
        {
            return locator =>
            {
                var expression = this.source.GetCode();
                var local = engine.CreateScriptSourceFromString(expression);

                engine.Runtime.LoadAssembly(this.GetType().Assembly);
                engine.Runtime.LoadAssembly(Assembly.GetExecutingAssembly());
                
                var factory = new Factory(engine);
                var scope = engine.CreateScope();

                scope.SetVariable("Factory", factory);

                var installer = engine.Execute(local.GetCode(), scope);
                var registration = installer.Install();

                locator.Register(registration);
            };
        }
    }

    public class Factory
    {
        private readonly ScriptEngine engine;

        public Factory(ScriptEngine engine)
        {
            this.engine = engine;
        }

        public object Create(object t)
        {
            return engine.Operations.CreateInstance(t);
        }
    }
}