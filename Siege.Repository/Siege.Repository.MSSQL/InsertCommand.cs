using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Siege.Repository.Mapping;
using Siege.Repository.Mapping.PropertyMappings;

namespace Siege.Repository.MSSQL
{
    public class InsertCommand : ICommand
    {
        private readonly DomainMap domainMap;

        public InsertCommand(DomainMap domainMap)
        {
            this.domainMap = domainMap;
        }

        public SqlCommand GenerateFor<T>(T item) where T : class
        {
            var command = new SqlCommand();
            var sql = new StringBuilder();

            var queries = GenerateQueriesFor(typeof (T), item);
            string ids = "";

            queries.ForEach(q =>
            {
                q.Parameters.ForEach(p =>
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = p.Name;
                    parameter.Value = p.Value;

                    command.Parameters.Add(parameter);
                });

                var id = command.CreateParameter();
                id.ParameterName = q.ID.Name;
                command.Parameters.Add(id);

                sql.Append(q.Sql);
                ids += q.ID.Name + ", ";
            });

            if (ids.LastIndexOf(',') > 0) ids = ids.Remove(ids.LastIndexOf(','));

            sql.Append("SELECT ");
            sql.Append(ids);
            sql.Append(";");
            command.CommandText = sql.ToString();

            return command;
        }

        private List<Query> GenerateQueriesFor(Type type, object item)
        {
            var queries = new List<Query>();

            var sql = "INSERT INTO {0}.{1} ({2}) VALUES ({3}); SELECT {4} = Scope_Identity(); ";

            var mappings = this.domainMap.Mappings.For(type);

            var schema = mappings.Table.Schema;
            var table = mappings.Table.Name;
            var columnList = "";
            var values = "";

            var identity = mappings.SubMappings.OfType<IdMapping>().First();
            var properties = mappings.SubMappings.OfType<IPropertyMapping>().Where(prop => !(prop is IdMapping) && !(prop is ForeignRelationshipMapping)).ToList();
            var components = mappings.SubMappings.OfType<ComponentMapping>().ToList();
            var entities = mappings.SubMappings.OfType<ForeignRelationshipMapping>().ToList();

            foreach(var entity in entities)
            {
                queries.AddRange(GenerateQueriesFor(entity.Property.PropertyType, entity.Property.GetValue(item, new object[0])));
            }

            var query = new Query();

            properties.ForEach(map =>
            {
                values += "@" + mappings.Table.Name + "_" + map.ColumnName + ", ";
                columnList += "[" + map.ColumnName + "], ";

                var parameter = new QueryParameter
                {
                    Name = "@" + mappings.Table.Name + "_" + map.ColumnName,
                    Value = map.Property.GetValue(item, new object[0])
                };

                query.Parameters.Add(parameter);
            });

            components.ForEach(c => c.SubMappings.OfType<IPropertyMapping>().ToList().ForEach(map =>
            {
                values += "@" + mappings.Table.Name + "_" + map.ColumnName + ", ";
                columnList += "[" + map.ColumnName + "], ";

                var parameter = new QueryParameter
                {
                    Name = "@" + mappings.Table.Name + "_" + map.ColumnName,
                    Value = map.Property.GetValue(c.Property.GetValue(item, new object[0]), new object[0])
                };

                query.Parameters.Add(parameter);
            }));

            entities.ForEach(map =>
            {
                var sourceTable = domainMap.Mappings.For(map.Property.PropertyType);
                values += "@" + sourceTable.Table.Name + "_" + map.ColumnName + ", ";
                columnList += "[" + map.ColumnName + "], ";
            });

            var id = new QueryParameter();
            var idParameterName = "@" + mappings.Table.Name + "_" + identity.ColumnName;

            id.Name = idParameterName;
            query.ID = id;

            if(columnList.LastIndexOf(',') > 0) columnList = columnList.Remove(columnList.LastIndexOf(','));
            if(values.LastIndexOf(',') > 0) values = values.Remove(values.LastIndexOf(','));

            query.Sql = string.Format(sql, schema, table, columnList, values, idParameterName);

            queries.Add(query);

            return queries;
        }

        //private List<IPropertyMapping> GetComponentProperties(List<IElementMapping> mappings)
        //{
        //    var items = new List<IPropertyMapping>();

        //    mappings.ForEach(map =>
        //    {
        //        if(map is IPropertyMapping) items.Add(map as IPropertyMapping);
        //        if(map is ComponentMapping) items.AddRange(GetComponentProperties(((ComponentMapping)map).SubMappings));
        //    });

        //    return items;
        //}
    }

    public class Query
    {
        public List<QueryParameter> Parameters { get; set; }
        public QueryParameter ID { get; set; } 
        public string Sql { get; set; }

        public Query()
        {
            this.Parameters = new List<QueryParameter>();
        }
    }

    public class QueryParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}