using System.Data;
using Npgsql;

namespace DataAccess
{
    public class PostgresDbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetDbConnection()
        {
            // TODO: pick the Connection String from the config.
            return new NpgsqlConnection("Server=localhost;Database=twitter;Port=5432;Userid=postgres;Password=admin");
        }
    }
}