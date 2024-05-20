using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WinFormsAppDEMO4
{
    public class DatabaseManager
    {
        private MySqlConnection connection;

        private const string server = "127.0.0.1";
        private const string database = "demodb";
        private const string uid = "root";
        private const string password = "Vfrcbvtdutybz20042010";

        public DatabaseManager()
        {
            InirializeDatabaseConnection();
        }

        private void InirializeDatabaseConnection()
        {
            string connectionString;
            connectionString = $"SERVER={server}; DATABASE={database}; UID={uid}; PASSWORD={password};";

            connection = new MySqlConnection(connectionString);
            connection.Open();
        }
        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
