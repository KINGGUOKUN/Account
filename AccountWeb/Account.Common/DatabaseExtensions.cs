using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace Account.Common
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Dapper扩展方法，执行复杂查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql">待执行sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        public static bool Execute(this IDatabase database, string sql, Dictionary<string, object> parameters = null, int? commandTimeOut = null)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }
            }

            return database.Connection.Execute(sql: sql, param: dynamicParameters, commandTimeout: commandTimeOut) > 0;
        }

        public static object ExecuteScalar(this IDatabase database, string sql, Dictionary<string, object> parameters = null, int? commandTimeOut = null)
        {
            using (IDbCommand command = database.Connection.CreateCommand())
            {
                command.CommandText = sql;
                if (commandTimeOut.HasValue)
                {
                    command.CommandTimeout = commandTimeOut.Value;
                }
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var pair in parameters)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = pair.Key;
                        parameter.Value = pair.Value;
                        command.Parameters.Add(parameter);
                    }
                }

                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Dapper扩展方法，执行存储过程(无返参数)
        /// </summary>
        /// <param name="database"></param>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        public static void ExecuteProcedure(this IDatabase database, string procedureName, Dictionary<string, object> parameters = null, int? commandTimeOut = null)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }
            }

            database.Connection.Execute(sql: procedureName, param: dynamicParameters, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeOut);
        }

        /// <summary>
        /// Dapper扩展方法，执行存储过程(带返回参数)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="procedureName"></param>
        /// <param name="parameters">传入参数</param>
        /// <param name="outParameterName">返回参数的名字</param>
        /// <returns></returns>
        public static T ExecuteProcedure<T>(this IDatabase database, string procedureName, Dictionary<string, object> parameters = null, string outParameterName = null, int? commandTimeOut = null)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }
            }

            database.Connection.Execute(sql: procedureName, param: dynamicParameters, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeOut);

            return dynamicParameters.Get<T>("outParameterName");
        }
    }
}
