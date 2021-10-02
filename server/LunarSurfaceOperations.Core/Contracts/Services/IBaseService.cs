namespace LunarSurfaceOperations.Core.Contracts.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using MongoDB.Bson;

    public interface IBaseService<in TPrototype, TLayout>
        where TLayout : ILayout
    {
        Task<IOperationResult<TLayout>> CreateAsync(TPrototype prototype, CancellationToken cancellationToken);
        Task<IOperationResult<TLayout>> UpdateAsync(ObjectId id, TPrototype prototype, CancellationToken cancellationToken);
    }
}