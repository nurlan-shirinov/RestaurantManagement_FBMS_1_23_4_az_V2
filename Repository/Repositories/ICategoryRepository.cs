using Domain.Entites;

namespace Repository.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task Update(Category category);
    Task<bool> Remove(int id , int deletedBy);
    IQueryable<Category> GetAll();
    Task<Category> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetByNameAsync(string name);
}