using System;
using FluentNHibernate;
using FluentNHibernate.Conventions;
using Siege.Repository.NHibernate.Conventions;

namespace Siege.Security.SQL.Conventions
{
    public class ForeignKeyNameConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            if (property == null)
                return type.Name + ConventionConstants.Id;

            return property.Name + ConventionConstants.Id;
        }
    }
}