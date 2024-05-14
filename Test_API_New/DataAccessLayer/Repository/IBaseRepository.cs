using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Xml.XPath;
using Test_API_New.DataAccessLayer.Context;
using Test_API_New.DataAccessLayer.Entities;

namespace Test_API_New.DataAccessLayer.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(int id, T entity);
        Task<T?> UpdatePatchAsync(int id, JsonPatchDocument<T> patchDocument);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(params string[] navsToInclude);
        Task<T?> GetById(int id);
    }

    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly StoreDBContext storeDBContext;
        private readonly DbSet<T> dbSet;
        public BaseRepository(StoreDBContext storeDBContext)
        {
            this.storeDBContext = storeDBContext;
            this.dbSet = this.storeDBContext.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            var result = await dbSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity is null) return await Task.FromResult(false);
            dbSet.Remove(entity);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] navsToInclude)
        {
            IQueryable<T> query = dbSet;
            if (navsToInclude.Length > 0)
            {
                foreach (var nav in navsToInclude)
                {
                    query = query.Include(nav);
                }
            }
            var result = query.AsEnumerable();
            return await Task.FromResult(result);
        }

        public async Task<T?> GetById(int id)
        {
            var entity = await dbSet.FindAsync(id);
            return entity;
        }

        public async Task<T?> UpdateAsync(int id, T entity)
        {
            var DBentity = await dbSet.FindAsync(id);
            if (DBentity is not null)
            {
                storeDBContext.Entry(DBentity).CurrentValues.SetValues(entity);
                return entity;
            }
            return DBentity;
        }

        public async Task<T?> UpdatePatchAsync(int id, JsonPatchDocument<T> patchDocument)
        {
            var DBentity = await dbSet.FindAsync(id);
            if (DBentity is not null)
            {
                patchDocument.ApplyTo(DBentity);
                return DBentity;
            }
            return null;
        }

    }

    public interface IUserRepository : IBaseRepository<User> { }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(StoreDBContext storeDBContext) : base(storeDBContext) { }
    }

    public interface IProductRepository : IBaseRepository<Product> { }

    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(StoreDBContext storeDBContext) : base(storeDBContext) { }
    }

    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; set; }
        IProductRepository ProductRepository { get; set; }
        Task<int> SaveAsync();
    }

    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly StoreDBContext storeDBContext;
        public IUserRepository UserRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }

        public RepositoryWrapper(StoreDBContext storeDBContext)
        {
            this.storeDBContext = storeDBContext;
            UserRepository = new UserRepository(storeDBContext);
            ProductRepository = new ProductRepository(storeDBContext);
        }

        public async Task<int> SaveAsync()
        {
            return await storeDBContext.SaveChangesAsync();
        }
    }
}
