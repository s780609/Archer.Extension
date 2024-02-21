using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Archer.Extension.DatabaseHelper
{
    public class DatabaseHelper
    {
        private IConfiguration _configuration;
        private IDbConnection _dbConnection;
        private readonly SecurityHelper _securityHelp;

        public DatabaseHelper(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public DatabaseHelper(IConfiguration configuration, SecurityHelper securityHelp)
        {
            _configuration = configuration;
            _securityHelp = securityHelp;
        }

        /// <summary>
        /// connection names are in the appsettings.json
        /// </summary>
        /// <param name="databaseType"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IDbConnection CreateConnectionBy(string connectionName, DatabaseType databaseType = DatabaseType.SQLServer)
        {
            if (_dbConnection != null)
            {
                return _dbConnection;
            }

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
