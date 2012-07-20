/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System.Collections.Generic;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using Siege.Repository.NHibernate.Conventions;

namespace Siege.Repository.NHibernate
{
    public class NHibernateConfigurationManager
    {
        public static bool ShowSql { get; set; }
        private static readonly Dictionary<string, FluentConfiguration> configurations;
        private static readonly Dictionary<string, List<IConvention>> conventions;
        private static readonly object configLock = new object();

        static NHibernateConfigurationManager()
        {
            configurations = new Dictionary<string, FluentConfiguration>();
            conventions = new Dictionary<string, List<IConvention>>();
        }

        public static FluentConfiguration GetConfiguration(string connectionStringName)
        {
            return GetConfiguration(connectionStringName, false);
        }

        public static FluentConfiguration GetConfiguration(string connectionStringName, bool showSql)
        {
            if (!configurations.ContainsKey(connectionStringName))
            {
                lock (configLock)
                {
                    if (!configurations.ContainsKey(connectionStringName))
                    {
                        configurations[connectionStringName] = Configure(connectionStringName, showSql);
                    }
                }
            }
            return configurations[connectionStringName];
        }

        public NHibernateConfigurationManager AddConvention<TConvention>(string connectionStringName) where TConvention : IConvention, new()
        {
            if(!conventions.ContainsKey(connectionStringName))
            {
                conventions.Add(connectionStringName, new List<IConvention>());
            }

            var list = conventions[connectionStringName];

            list.Add(new TConvention());
            conventions.Add(connectionStringName, list);

            return this;
        }

        private static FluentConfiguration Configure(string connectionStringName, bool showSql)
        {
            return Fluently
                .Configure()
                .Database(DatabaseConfigurationFor(connectionStringName, showSql))
                    .Mappings(m => m.FluentMappings
                                   .Conventions.Add(conventions[connectionStringName].ToArray())
                                   .Conventions.AddFromAssemblyOf<ClassConvention>());
        }

        private static IPersistenceConfigurer DatabaseConfigurationFor(string connectionStringName, bool showSql)
        {
            var config = MsSqlConfiguration.MsSql2005;
            if (showSql) config = config.ShowSql();
            config = config.ConnectionString(c => c.FromConnectionStringWithKey(connectionStringName));
            return config;
        }
    }

}
