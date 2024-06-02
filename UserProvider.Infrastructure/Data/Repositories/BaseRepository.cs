using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace UserProvider.Infrastructure.Data.Repositories;

public class BaseRepository<TEntity, TContext>(IDbContextFactory<TContext> contextFactory, ILogger<BaseRepository<TEntity, TContext>> logger) where TEntity : class where TContext : DbContext
{
    private readonly IDbContextFactory<TContext> _contextFactory = contextFactory;
    private readonly ILogger<BaseRepository<TEntity, TContext>> _logger = logger;


    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Set<TEntity>().AnyAsync(predicate);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.ExistsAsync() :: {ex.Message}");
        }
        return false;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.CreateAsync() :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.GetOneAsync() :: {ex.Message}");
        }
        return null!;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.Set<TEntity>().ToListAsync();
            return entities;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.GetAllAsync() :: {ex.Message}");
        }
        return null!;
    }

    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null && updatedEntity != null)
            {
                context.Entry(entity).CurrentValues.SetValues(updatedEntity);
                await context.SaveChangesAsync();
                return entity;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.UpdateAsync() :: {ex.Message}");
        }
        return null!;
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                context.Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.DeleteAsync() :: {ex.Message}");
        }
        return false;
    }


}
   
