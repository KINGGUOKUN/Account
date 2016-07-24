using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Account.Common
{
    public static class DatabaseExtensions
    {
        public static IEnumerable<T> GetList<T>(this IDatabase database, string sql, Dictionary<string, object> parameters)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }
            }

            return database.Connection.Query<T>(sql, dynamicParameters);
        }
    }
}
