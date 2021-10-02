namespace LunarSurfaceOperations.Data.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using MongoDB.Bson;

    public interface IRepository<TEntity>
        where TEntity : class, IEntity
    {
        Task<IOperationResult<TEntity>> GetAsync(ObjectId id, CancellationToken cancellationToken);
        Task<IOperationResult> CreateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<IOperationResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}