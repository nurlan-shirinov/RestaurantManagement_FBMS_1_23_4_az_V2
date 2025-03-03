using DAL.SqlServer.Context;
using DAL.SqlServer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Common;

namespace DAL.SqlServer;

public static class DependencyInjections
{
    public static IServiceCollection AddSqlServerServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, SqlUnitOfWork>(opt =>
        {
            var dbContext = opt.GetRequiredService<AppDbContext>();
            return new SqlUnitOfWork(connectionString, dbContext);
        });

        return services;
    }
}