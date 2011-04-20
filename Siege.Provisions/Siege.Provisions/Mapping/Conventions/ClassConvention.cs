using System;
using System.Collections.Generic;
using System.Reflection;
using Siege.Provisions.Mapping.Conventions.Formatters;
using Siege.Provisions.Mapping.Conventions.Handlers;
using Siege.Provisions.Mapping.Conventions.Identifiers;

namespace Siege.Provisions.Mapping.Conventions
{
    public class ClassConvention : IConvention
    {
        private readonly DomainMapper masterMap;
        private readonly Formatter<Type> primaryKeyFormatter = new Formatter<Type>();
        private readonly Formatter<PropertyInfo> foreignKeyFormatter = new Formatter<PropertyInfo>();
        private readonly ComponentHandler componentHandler;
        private readonly List<IHandler> handlers = new List<IHandler>();

        private string prefix = "";
        private string schema = "";
        private string suffix = "";

        public ClassConvention(DomainMapper masterMap)
        {
            this.masterMap = masterMap;
            this.componentHandler = new ComponentHandler();
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

        public void Map(Type type, DomainMapping mapper)
        {
            this.handlers.Add(componentHandler);
            this.handlers.Add(new PropertyHandler());
            this.handlers.Add(new OneToManyHandler(foreignKeyFormatter, masterMap));

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

        public void IdentifyEntitiesWith<TIdentifier>() where TIdentifier : class, IIdentifier<Type>, new()
        {
            this.handlers.Add(new EntityHandler(new TIdentifier(), foreignKeyFormatter, masterMap));
        }

        public void IdentifyEntitiesWith(Predicate<Type> entityMatcher)
        {
            this.handlers.Add(new EntityHandler(new InlineIdentifier<Type>(entityMatcher), foreignKeyFormatter, masterMap));
        }

        public void IdentifyComponentsWith<TIdentifier>() where TIdentifier : class, IIdentifier<Type>, new()
        {
            componentHandler.MatchesOn(new TIdentifier());
        }

        public void IdentifyComponentsWith(Predicate<Type> componentMatcher)
        {
            componentHandler.MatchesOn(new InlineIdentifier<Type>(componentMatcher));
        }

        public void IdentifyIDsWith(Predicate<PropertyInfo> idMatcher)
        {
            this.handlers.Add(new IDHandler(new InlineIdentifier<PropertyInfo>(idMatcher), primaryKeyFormatter));
        }

        public void IdentifyIDsWith<TIdentifier>() where TIdentifier : class, IIdentifier<PropertyInfo>, new()
        {
            this.handlers.Add(new IDHandler(new TIdentifier(), primaryKeyFormatter));
        }

        public void ForForeignKeys(Action<Formatter<PropertyInfo>> foreignKey)
        {
            foreignKey(foreignKeyFormatter);
        }

        public void ForPrimaryKeys(Action<Formatter<Type>> primaryKey)
        {
            primaryKey(primaryKeyFormatter);
        }
    }
}