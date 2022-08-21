using Microsoft.EntityFrameworkCore;
using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Library.Common.Repos
{
    public abstract class ArticlesBaseRepo<T> : IRepo<T> where T : class, IIdentifiable, new()
    {
        public ArticlesBaseRepo(ArticlesDbContext context)
        {
            Context = context;
        }

        public ArticlesDbContext Context { get; init; }

        protected DbSet<T> Table;

        public async Task<int> AddAsync(T entity)
        {
            await Table.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            await Table.AddRangeAsync(entities);
            return await SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var entity = GetOneAsync(id);

            if (entity != null)
            {
                Context.Entry(entity).State = EntityState.Deleted;
                return await SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            int result = 0;

            foreach (var id in ids)
            {
                var affectedValue = await DeleteAsync(id);
                result += affectedValue;
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await Table.ToListAsync();

        public async Task<T> GetOneAsync(Guid id) => await Task.Run(() => Table.FirstOrDefault(entity => entity.Id == id));

        public async Task<int> SaveAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return await SaveChangesAsync();
        }

        internal async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
