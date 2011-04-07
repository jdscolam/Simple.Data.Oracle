﻿using System;
using System.Collections.Generic;
using Simple.Data.Ado.Schema;
using System.Linq;

namespace Simple.Data.Oracle
{
    internal class OracleSchemaProvider : ISchemaProvider
    {
        private readonly SqlReflection _sqlReflection;

        public OracleSchemaProvider(OracleConnectionProvider connectionProvider)
        {
            _sqlReflection = new SqlReflection(connectionProvider);
            //http://www.devart.com/dotconnect/oracle/docs/DataTypeMapping.html
        }

        public IEnumerable<Table> GetTables()
        {
            return _sqlReflection.Tables.AsEnumerable();
        }

        public IEnumerable<Column> GetColumns(Table table)
        {
            return _sqlReflection.Columns
                .Where(c => table.ActualName.InvariantEquals(c.Item1))
                .Select(c => new Column(c.Item2, table));
        }

        public Key GetPrimaryKey(Table table)
        {
            return new Key(_sqlReflection.PrimaryKeys.Where(t => t.Item1.InvariantEquals(table.ActualName)).Select(t => t.Item2));
        }

        public IEnumerable<ForeignKey> GetForeignKeys(Table table)
        {
            return _sqlReflection.ForeignKeys.Where(fk => fk.DetailTable.Name.InvariantEquals(table.ActualName));
        }

        public IEnumerable<Procedure> GetStoredProcedures()
        {
            //new Procedure()
            throw new NotImplementedException();
        }

        public IEnumerable<Parameter> GetParameters(Procedure storedProcedure)
        {
            
            throw new NotImplementedException();
        }

        public string QuoteObjectName(string unquotedName)
        {
            return string.Format("\"{0}\"", unquotedName);
        }

        public string NameParameter(string baseName)
        {
            if (string.IsNullOrWhiteSpace(baseName))
                throw new ArgumentException("baseName is not set.");
            return (baseName.StartsWith(":")) ? baseName : ":" + baseName;
        }
    }
}