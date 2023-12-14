using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HealthyLife.Context
{
    public class DapperContext : DbContext
    {
        private readonly string _connectionString;
        public DapperContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}