using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.Connection
{
    public class SqlDbConnection
    {
        string _connectionString = null;
        string _connectionSettingName = "Tesseract_ConnectionString";

        public string GetConnectionString(string connectionSettingName = null)
        {
            _connectionSettingName = connectionSettingName ?? _connectionSettingName;

            var connectionStringSection = ConfigurationManager.ConnectionStrings[_connectionSettingName];

            return connectionStringSection.ConnectionString;
        }

        public SqlConnection CreateConnection(string connectionString = null, string connectionSettingName = null)
        {
            _connectionSettingName = connectionString ?? _connectionSettingName;
            _connectionString = connectionString ?? GetConnectionString(_connectionSettingName);

            var sqlConnection = new SqlConnection(_connectionString);

            return sqlConnection;
        }

        public bool OpenConnection_Test(string connectionString)
        {
            using (var connection = CreateConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (Exception exception)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
