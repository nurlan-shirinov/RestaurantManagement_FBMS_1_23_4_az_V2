using Domain.Entites;

namespace Repository.Repositories;

public interface IUserRepository
{
    Task RegisterAsync(User user);
    void Update(User user);
    void Remove(int id);
    IQueryable<User> GetAll();
    Task<User> GetByIdAsync(int id);
    Task<User> GetByEmailAsync(string email);
}