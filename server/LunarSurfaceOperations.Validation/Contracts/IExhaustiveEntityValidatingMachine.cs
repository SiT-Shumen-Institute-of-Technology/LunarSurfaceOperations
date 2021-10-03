namespace LunarSurfaceOperations.Validation.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Utilities.OperationResults;

    public interface IExhaustiveEntityValidatingMachine
    {
        Task<IOperationResult> TriggerValidationAsync(CancellationToken cancellationToken);
    }
}