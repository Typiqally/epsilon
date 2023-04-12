namespace NetCore.Lti.Data;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
    where TEntity : class, IEntity
{
    TEntity Create(TEntity entity);
    Task<TEntity> CreateAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(object id);
    Task DeleteAsync(object id);
    void Delete(TEntity? entity);
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}