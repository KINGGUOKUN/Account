using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Account.DAL
{
    public class DatabaseFactory
    {
        #region Singleton

        private static Lazy<DatabaseFactory> _instance = new Lazy<DatabaseFactory>(() => new DatabaseFactory());

        public static DatabaseFactory Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private DatabaseFactory()
        {
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// 创建的数据库集合
        /// </summary>
        private Dictionary<string, IDatabase> _dictDatabase = new Dictionary<string, IDatabase>();

        private static object _createLock = new object();

        #endregion

        /// <summary>
        /// 根据配置文件构造数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IDatabase CreateDatabase(string connectionString = "default")
        {
            lock(_createLock)
            {
                if (_dictDatabase.ContainsKey(connectionString))
                {
                    return _dictDatabase[connectionString];
                }
                var connectionStringSettions = ConfigurationManager.ConnectionStrings[connectionString];
                if (connectionStringSettions == null || !string.Equals(connectionStringSettions.ProviderName, "System.Data.SqlClient", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("数据库连接配置异常，请核查！", "connectionString");
                }

                var connection = new SqlConnection(connectionStringSettions.ConnectionString);
                var sqlGeneratorIml = new SqlGeneratorImpl(
                    new DapperExtensionsConfiguration(typeof(AutoClassMapper<>),
                    new List<Assembly>(),
                    new SqlServerDialect()));
                IDatabase database = new Database(connection, sqlGeneratorIml);
                _dictDatabase.Add(connectionString, database);

                return database;
            }
        }
    }
}
