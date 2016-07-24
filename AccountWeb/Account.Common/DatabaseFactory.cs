using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Account.Common
{
    public class DatabaseFactory
    {
        #region Private Fields

        private static readonly ConcurrentDictionary<string, IDatabase> _databases = new ConcurrentDictionary<string, IDatabase>();

        #endregion

        #region Public Methods

        public static IDatabase CreateDatabase()
        {
            return CreateDatabase("default");
        }

        public static IDatabase CreateDatabase(string connectionName)
        {
            if (_databases.ContainsKey(connectionName))
            {
                return _databases[connectionName];
            }
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

            IDatabase database = new Database(new SqlConnection(connectionString),
                new SqlGeneratorImpl(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>),
                new List<Assembly>(), new SqlServerDialect())));
            _databases.TryAdd(connectionName, database);

            return database;
        }

        #endregion
    }
}