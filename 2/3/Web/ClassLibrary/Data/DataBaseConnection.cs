using System.Data.Common;
using System.Data.SqlClient;

namespace InsuranceLib.Data

{
    public class DataBaseConnection
    {
        private DbConnection _connection;
        private static DataBaseConnection _instance;

        public static DataBaseConnection Instance
        {
            get
            {
                return _instance;
            }
        }
        static DataBaseConnection()
        {
            _instance = new DataBaseConnection();
        }

        private DataBaseConnection()
        {
            string connectionString = "Data Source=DESKTOP-17DJJOK;Initial Catalog=master;Database=Insurance;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            _connection = new SqlConnection(connectionString);
        }

        public DbConnection GetConnection()
        {
            return _connection;
        }
    }
}

