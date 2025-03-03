using DAL.SqlServer.Context;
using Dapper;
using Domain.Entites;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;

public class SqlCategoryRepository : BaseSqlRepository, ICategoryRepository
{
    private readonly AppDbContext _context;
    public SqlCategoryRepository(string connectionString, AppDbContext context) : base(connectionString)
    {
        _context = context;
    }

    public async Task AddAsync(Category category)
    {
        var sql = @"INSERT INTO CATEGORIES([Name],[CreatedBy])
                    VALUES(@Name , @CreatedBy); SELECT SCOPE_IDENTITY()";

        using var conn = OpenConnection();
        var generatedId = await conn.ExecuteScalarAsync<int>(sql, category);
        category.Id = generatedId;
    }

    public IQueryable<Category> GetAll()
    {
        return _context.Categories.OrderByDescending(c => c.CreatedDate).Where(c => c.IsDeleted == false);
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        var sql = @"SELECT c.*
                    FROM CATEGORIES c
                    WHERE c.Id=@id AND c.IsDeleted=0";

        using var conn = OpenConnection();

        return await conn.QueryFirstOrDefaultAsync<Category>(sql, new { id });
    }

    public async Task<IEnumerable<Category>> GetByNameAsync(string name)
    {
        var sql = @"DECLARE @searchText NVARCHAR(max)
                    SET @searchText = '%'+ @name + '%'
                    SELECT c.* 
                    FROM Categories c
                    WHERE c.[Name] Like @searchText and c.IsDeleted=0";

        using var conn = OpenConnection();
        return await conn.QueryAsync<Category>(sql, name);
    }

    public async Task<bool> Remove(int id, int deletedBy)
    {
        var checkSql = "SELECT Id FROM Categories WHERE Id=@id AND IsDeleted=0";

        var sql = @"UPDATE Categories
                    SET IsDeleted=1,
                    DeletedBy= @deletedBy,
                    DeletedDate = GETDATE()
                    Where Id=@id";

        using var conn = OpenConnection();
        using var transaction = conn.BeginTransaction();

        var categoryId = await conn.ExecuteScalarAsync<int?>(checkSql, new { id } , transaction);

        if (!categoryId.HasValue)
                return false;

        var affectedRow = await conn.ExecuteAsync(sql, new { id, deletedBy } , transaction);

        transaction.Commit();

        return affectedRow > 0;

    }

    public async Task Update(Category category)
    {
        var sql = @"UPDATE Categories
                    SET Name = @Name,
                    UpdatedBy= @UpdatedBy,
                    UpdatedDate = GETDATE()
                    WHERE Id = @Id";

        using var conn = OpenConnection();
        await conn.QueryAsync<Category>(sql, category);
    }
}
