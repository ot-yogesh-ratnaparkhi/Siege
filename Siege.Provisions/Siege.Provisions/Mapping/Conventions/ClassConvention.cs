using System;
using System.Collections.Generic;
using Siege.Provisions.Mapping.Conventions.Handlers;

namespace Siege.Provisions.Mapping.Conventions
{
    public class ClassConvention : IConvention
    {
        private readonly List<IHandler> handlers = new List<IHandler>();

        private string prefix = "";
        private string schema = "";
        private string suffix = "";

        public ClassConvention()
        {
            this.handlers.Add(new PropertyHandler());
        }

        public ClassConvention WithSchema(string schema)
        {
            this.schema = schema;

            return this;
        }

        public ClassConvention WithPrefix(string prefix)
        {
            this.prefix = prefix;

            return this;
        }

        public ClassConvention WithSuffix(string suffix)
        {
            this.suffix = suffix;

            return this;
        }

        public void Map(Type type, DomainMapper mapper)
        {
            foreach(var property in type.GetProperties())
            {
                foreach(var handler in this.handlers)
                {
                    if(!handler.CanHandle(property)) continue;

                    handler.Handle(property, type, mapper);
                }
            }

            mapper.For(type).Table.Schema = this.schema;
            mapper.For(type).Table.Name = this.prefix + type.Name + this.suffix;
        }

        public void ForComponents(Action<ComponentConvention> convention)
        {
            
        }
    }
}