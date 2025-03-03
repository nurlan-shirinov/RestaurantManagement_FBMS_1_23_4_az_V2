using Repository.Repositories;

namespace Repository.Common;

public interface IUnitOfWork
{
    public ICategoryRepository CategoryRepository { get;}
    public IUserRepository UserRepository { get;}

    Task<int> SaveChanges();
}