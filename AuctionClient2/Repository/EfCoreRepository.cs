
using System.Linq.Expressions;
using AuctionClient3.DbContext;
using AuctionClient3.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionClient3.Repository;

public abstract class EfCoreRepository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : ClientContext
{
    private readonly TContext context;
    public EfCoreRepository(TContext context)
    {
        this.context = context;
    }
    public async Task<TEntity> Add(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> Delete(int id)
    {
        var entity = await context.Set<TEntity>().FindAsync(id);
        if (entity == null)
        {
            return entity;
        }

        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> Get(int id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    // LastOrDefault implementation
    public async Task<TEntity> LastOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().LastOrDefaultAsync(predicate);
    }

    // OrderByDescending implementation
    public async Task<List<TEntity>> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
    {
        return await context.Set<TEntity>().OrderByDescending(keySelector).ToListAsync();
    }
    public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().AnyAsync(predicate);
    }

    // Count implementation
    public async Task<int> Count(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().CountAsync(predicate);
    }
}