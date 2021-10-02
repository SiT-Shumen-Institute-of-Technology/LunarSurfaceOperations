namespace LunarSurfaceOperations.Core.Contracts.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using MongoDB.Bson;

    public interface IBaseService<in TPrototype, TLayout>
    {
        Task<IOperationResult<TLayout>> CreateAsync(TPrototype prototype, CancellationToken cancellationToken);
        Task<IOperationResult<TLayout>> UpdateAsync(ObjectId id, TPrototype prototype, CancellationToken cancellationToken);
    }
}