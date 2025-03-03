using DAL.SqlServer.Context;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;

public class SqlUserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public IQueryable<User> GetAll()
    {
        return _context.Users.OrderByDescending(u => u.CreatedDate).Where(u => u.IsDeleted == false);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task RegisterAsync(User user)
    {
        await _context.AddAsync(user);
    }

    public void Remove(int id)
    {
        var currentUser = _context.Users.FirstOrDefault(u => u.Id == id);
        currentUser.IsDeleted=true;
        currentUser.DeletedDate=DateTime.Now;
        currentUser.DeletedBy = 1;
    }

    public void Update(User user)
    {
        user.UpdatedDate = DateTime.Now;
        _context.Update(user);
    }
}
