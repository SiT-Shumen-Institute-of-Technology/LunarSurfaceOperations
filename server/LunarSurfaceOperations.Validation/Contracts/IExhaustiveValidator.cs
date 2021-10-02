namespace LunarSurfaceOperations.Validation.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Utilities.OperationResults;

    public interface IExhaustiveValidator<in TEntity>
    {
        Task<IOperationResult> ValidateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}