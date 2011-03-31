using System;
using System.Collections.Generic;
using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Handlers;

namespace Siege.Provisions.Mapping.Conventions
{
    public class ClassConvention : IConvention
    {
        private readonly ComponentHandler componentHandler = new ComponentHandler();
        private readonly List<IHandler> handlers = new List<IHandler>();

        private string prefix = "";
        private string schema = "";
        private string suffix = "";

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

        public void Map(Type type, DomainMapping mapper)
        {
            this.handlers.Add(componentHandler);
            this.handlers.Add(new PropertyHandler());
            this.handlers.Add(new OneToManyHandler());

            foreach(var property in type.GetProperties())
            {
                foreach (var handler in this.handlers)
                {
                    if (handler.CanHandle(property))
                    {
                        handler.Handle(property, type, mapper);
                        break;
                    }
                }
            }

            mapper.Table.Schema = this.schema;
            mapper.Table.Name = this.prefix + type.Name + this.suffix;
        }

        public void ForComponents(Action<ComponentConvention> convention)
        {
            this.componentHandler.HandlesWith(convention);
        }

        public void IsEntityWhen(Predicate<Type> entityMatcher)
        {
            this.handlers.Add(new EntityHandler(entityMatcher));
        }

        public void IsComponentWhen(Predicate<Type> componentMatcher)
        {
            componentHandler.MatchesOn(componentMatcher);
        }

        public void MatchIDWith(Predicate<PropertyInfo> idMatcher)
        {
            this.handlers.Add(new IDHandler(idMatcher));
        }
    }
}