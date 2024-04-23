using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Archer.Extension.DatabaseHelper
{
    public class DatabaseHelper
    {
        private IConfiguration _configuration;
        private readonly SecurityHelper.SecurityHelper _securityHelp;

        public DatabaseHelper(SecurityHelper.SecurityHelper securityHelp)
        {
            _securityHelp = securityHelp;
        }

        public DatabaseHelper(IConfiguration configuration, SecurityHelper.SecurityHelper securityHelp)
        {
            _configuration = configuration;
            _securityHelp = securityHelp;
        }

        /// <summary>
        /// create connection
        /// </summary>
        /// <param name="databaseType">DB</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IDbConnection CreateIDbConnection(string connectionString, DatabaseType databaseType = DatabaseType.SQLServer)
        {
            CreateConnection createConnection;

            switch (databaseType)
            {
                case DatabaseType.SQLServer:
                    createConnection = CreateSqlConnection;
                    break;
                case DatabaseType.SQLite:
                    createConnection = CreateSqliteConnection;
                    break;
                default:
                    throw new ArgumentException($"{nameof(databaseType)} not found");
            }

            return createConnection(connectionString);
        }

        /// <summary>
        /// create connection by appsetting's conenction name
        /// </summary>
        /// <param name="databaseType"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IDbConnection CreateConnectionBy(string connectionName, DatabaseType databaseType = DatabaseType.SQLServer)
        {
            string conn = _configuration.GetConnectionString(connectionName);

            if (string.IsNullOrWhiteSpace(conn))
            {
                conn = string.Empty;
            }

            CreateConnection createConnection;

            switch (databaseType)
            {
                case DatabaseType.SQLServer:
                    createConnection = CreateSqlConnection;
                    break;
                case DatabaseType.SQLite:
                    createConnection = CreateSqliteConnection;
                    break;
                default:
                    throw new ArgumentException($"{nameof(databaseType)} not found");
            }

            return createConnection(conn);
        }

        private delegate IDbConnection CreateConnection(string connectionName);

        private SqlConnection CreateSqlConnection(string connection)
        {
            return new SqlConnection(_securityHelp.DecryptConn(connection));
        }

        private SqliteConnection CreateSqliteConnection(string connection)
        {
            return new SqliteConnection(_securityHelp.DecryptConn(connection));
        }
    }
}
