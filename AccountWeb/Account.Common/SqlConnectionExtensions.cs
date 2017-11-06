using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Account.Common
{
    public static class SqlConnectionExtensions
    {
        public static void BulkCopy<T>(this SqlConnection connection, List<T> models, int batchSize, string destinationTableName = null, int? bulkCopyTimeout = null, SqlTransaction transaction = null)
        {
            if (string.IsNullOrWhiteSpace(destinationTableName))
            {
                destinationTableName = typeof(T).Name;
            }

            DataTable dtToWrite = ToSqlBulkCopyDataTable(models, connection, destinationTableName);
            SqlBulkCopy sbc = null;
            if (transaction != null)
            {
                sbc = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
            }
            else
            {
                sbc = new SqlBulkCopy(connection);
            }
            using (sbc)
            {
                sbc.BatchSize = batchSize;
                sbc.DestinationTableName = destinationTableName;
                if (bulkCopyTimeout.HasValue)
                {
                    sbc.BulkCopyTimeout = bulkCopyTimeout.Value;
                }
                sbc.WriteToServer(dtToWrite);
            }
        }

        public static DataTable ToSqlBulkCopyDataTable<T>(List<T> models, SqlConnection connection, string tableName)
        {
            DataTable dt = new DataTable();
            Type modelType = typeof(T);
            List<SysColumn> columns = GetTableColumns(connection, tableName);
            List<PropertyInfo> mappingProps = new List<PropertyInfo>();
            var props = modelType.GetProperties();
            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                PropertyInfo mappingProp = props.FirstOrDefault(x => x.Name == column.Name
                || string.Equals(GetColumnAttributeValue(x), column.Name, StringComparison.OrdinalIgnoreCase));
                if (mappingProp == null)
                {
                    throw new Exception($"model类型'{modelType.FullName}'未定义与表'{tableName}'列名为'{column.Name}'映射的属性");
                }
                mappingProps.Add(mappingProp);
                Type dataType = GetUnderlyingType(mappingProp.PropertyType);
                if (dataType.IsEnum)
                {
                    dataType = typeof(int);
                }
                dt.Columns.Add(new DataColumn(column.Name, dataType));
            }

            foreach (var model in models)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < mappingProps.Count; i++)
                {
                    PropertyInfo prop = mappingProps[i];
                    object value = prop.GetValue(model, null);
                    if (GetUnderlyingType(prop.PropertyType).IsEnum)
                    {
                        if (value != null)
                        {
                            value = (int)value;
                        }
                    }
                    dr[i] = value ?? DBNull.Value;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private static List<SysColumn> GetTableColumns(SqlConnection connection, string tableName)
        {
            string sql = string.Format(@"select * from syscolumns
                                         inner join sysobjects on syscolumns.id == sysobjects.id
                                         where sysobjects.xtype = 'U'
                                         and sysobjects.name = '{0}'
                                         order by syscolumns.colid asc", tableName);

            List<SysColumn> columns = new List<SysColumn>();
            using (SqlConnection conn = ((ICloneable)connection).Clone() as SqlConnection)
            {
                conn.Open();
                using (var command = new SqlCommand(sql, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SysColumn column = new SysColumn();
                            column.Name = reader["name"] as string;
                            column.ColOrder = Convert.ToInt32(reader["colorder"]);
                            columns.Add(column);
                        }
                    }
                }
            }

            return columns;
        }

        private static Type GetUnderlyingType(Type type)
        {
            Type unType = Nullable.GetUnderlyingType(type);

            return unType ?? type;
        }

        private static string GetColumnAttributeValue(PropertyInfo prop)
        {
            return null;
        }

        class SysColumn
        {
            public string Name { get; set; }

            public int ColOrder { get; set; }
        }
    }
}
