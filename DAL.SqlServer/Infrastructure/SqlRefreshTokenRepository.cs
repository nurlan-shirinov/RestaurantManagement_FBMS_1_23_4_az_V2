using DAL.SqlServer.Context;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;

public class SqlRefreshTokenRepository(AppDbContext context) : IRefreshtokenRepository
{
    public async Task<RefreshToken> GetStoredRefreshToken(string refreshToken)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
    }

    public async Task SaveRefreshToken(RefreshToken refreshToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken);
    }
}
