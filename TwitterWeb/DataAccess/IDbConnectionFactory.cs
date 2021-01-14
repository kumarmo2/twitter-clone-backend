using System.Data;

namespace TwitterWeb.DataAccess
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetDbConnection();
    }
}