using Microsoft.EntityFrameworkCore;
using NetCore.Lti.Data;

namespace NetCore.Lti.EntityFrameworkCore.Repositories;

public class EntityFrameworkRepository<TContext, TEntity> : EntityFrameworkReadOnlyRepository<TContext, TEntity>, IRepository<TEntity>
    where TContext : DbContext
    where TEntity : class, IEntity
{
    public EntityFrameworkRepository(TContext context) : base(context)
    {
    }

    public TEntity Create(TEntity entity)
    {
        var created = Context.Set<TEntity>().Add(entity);
        return created.Entity;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var created = await Context.Set<TEntity>().AddAsync(entity);
        return created.Entity;
    }

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(object id)
    {
        var entity = Find(id);
        Delete(entity);
    }

    public async Task DeleteAsync(object id)
    {
        var entity = await FindAsync(id);
        Delete(entity);
    }

    public void Delete(TEntity? entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public int SaveChanges()
    {
        return Context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }
}