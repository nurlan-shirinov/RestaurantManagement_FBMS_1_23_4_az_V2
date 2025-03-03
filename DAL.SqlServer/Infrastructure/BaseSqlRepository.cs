using Microsoft.Data.SqlClient;

namespace DAL.SqlServer.Infrastructure;

public abstract class BaseSqlRepository
{
    private readonly string _connectionString;

    internal BaseSqlRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected SqlConnection OpenConnection()
    {
        var conn = new SqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}